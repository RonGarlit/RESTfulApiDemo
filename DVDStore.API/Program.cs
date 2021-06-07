// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: Program.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  Program class for starting the DVDStore API.
// **
// **  Change History
// **
// **  WHEN         WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-10-28   rgarlit     STARTED DEVELOPMENT Rewrite of API Prototype
// **  2020-10-29   rgarlit     Replaced .NET Core internal logging with NLog
// **  2020-11-12   rgarlit     Converted to .NET 5.0 and got Unint Testing working
// **  2020-11-13   rgarlit     Fix EF Core Obsolete stuff and added Swashbuckle/Swagger
// ***********************************************************************************/

using DVDStore.Common.ResourceParameters.v1_0;
using DVDStore.DAL.Context;
using DVDStore.DAL.Repositories.v1_0;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Linq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace DVDStore.API
{
	/// <summary>
	///     Program
	/// </summary>
	public class Program
	{
#if EFCoreLogging

		#region Public Fields

		/// <summary>
		///     EfNLogLoggerFactory
		/// </summary>
		/// <remarks>
		///     Used to tie Entity Framework Core logging into NLog
		/// </remarks>
		public static readonly LoggerFactory EfNLogLoggerFactory
			= new LoggerFactory(new[] { new NLogLoggerProvider() });

		#endregion Public Fields

#endif

		#region Public Methods

		/// <summary>
		///     CreateHostBuilder
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
				// Configure NLog here
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.SetMinimumLevel(LogLevel.Trace);
				})
				.UseNLog(); // NLog: Setup NLog for Dependency injection
		}

		/// <summary>
		///     Main
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			// Setup NLog for this application
			var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

			try
			{
				logger.Info("RESTfulApiPrototype Application Starting Up");

				// Build the host
				var host = CreateHostBuilder(args).Build();

				// Lets display some data from the app settings json file
				logger.Debug("Logging with NLog logger from the Program Main...");
				logger.Debug("=====================================================");
				logger.Debug("=== DATA: from Appsettings.json via Configuration ===");
				logger.Debug("=====================================================");
				logger.Debug($"AppName: {Startup.Configuration["MyApplication:Name"]}");
				logger.Debug($"    URL: {Startup.Configuration["MyApplication:DefaultUrl"]}");
				logger.Debug($"  Email: {Startup.Configuration["MyApplication:Support:Email"]}");
				logger.Debug($"  Phone: {Startup.Configuration["MyApplication:Support:Phone"]}");
				logger.Debug($" AppVer: {Startup.Configuration["MyApplication:Version"]}");
				logger.Debug("=====================================================");
				logger.Debug($"ConnectionSting Used: {Startup.Configuration["ConnectionStrings:DVDStoreDb"]}");
				logger.Debug("=====================================================");

				// Just some paranoid tests to reassure myself
				//TestDvdRepository();

				// Display Swagger link:
				//logger.Info($"Instead of Postman you can call Swagger in the browser with http://localhost:51000/swagger/index.html ");
				Console.BackgroundColor = ConsoleColor.Yellow;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($"Instead of Postman you can call Swagger in the browser with http://localhost:51000/swagger/index.html ");
				Console.ResetColor();
				host.Run();
			}
			catch (Exception ex)
			{
				//NLog: catch setup errors
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// Shutdown NLog LogManager: We need to make sure to flush
				// and stop internal timers/threads before application-exit
				// (***** Needed to avoid segmentation fault on Linux *****)
				LogManager.Shutdown();
			}
		} // END of public static void Main(string[] args)

		//=====================================================================

		// END of CreateHostBuilder

		//=====================================================================

		/// <summary>
		///     TestDvdRepository
		/// </summary>
		/// <remarks>
		///     This is used to test my back-end stuff prior to building
		///     controllers for API's.  Disable it in main when NOT testing
		///     but I'm leaving this here as an example.
		/// </remarks>
		public static void TestDvdRepository()
		{
			// This is just to test some changes I did to the repository and DbContext
			// Added DI for those changes and I was just paranoid and wanted to see proof
			Console.WriteLine("In TestDvdRepository");
			var connectionString = Startup.Configuration["ConnectionStrings:DVDStoreDb"];
			Console.WriteLine($"ConnectionString: {connectionString}");

			//=================================================================
			// THIS IS HOW YOU BUILD THE 'DbContextOptions'
			//=================================================================
			var options = new DbContextOptionsBuilder<DVDStoreDBContext>()
#if EFCoreLogging
				.UseLoggerFactory(EfNLogLoggerFactory) // Tie EF Core Logging into NLOG
				.EnableSensitiveDataLogging() // So you can see the EF SQL Parameter values
#endif
				.UseSqlServer(connectionString)
				.Options;
			//=================================================================

			// Regular DbContext - Normal Dotnet Created DbContext
			var dvdDb = new DVDStoreDBContext(options);

			using (var repo = new DvdStoreRepository(dvdDb))
			{
				var actorResourceParameters = new ActorResourceParameters();
				actorResourceParameters.PageNumber = 1;
				actorResourceParameters.PageSize = 20;
				// Test OrderBy Options
				//actorResourceParameters.OrderBy = "Actorid";
				actorResourceParameters.OrderBy = "Firstname";
				//actorResourceParameters.OrderBy = "Lastname";

				// Test Search Query Parameters
				//actorResourceParameters.SearchQuery = "BLOOM";
				//actorResourceParameters.SearchQuery = "BLO";
				//actorResourceParameters.SearchQuery = "VAL";
				//actorResourceParameters.SearchQuery = "Val";

				var getActorList = repo.GetActors(actorResourceParameters);
				var actorCount = getActorList.Count();
				Console.WriteLine($"Actor Table Record Count:  {actorCount}");
				//Console.WriteLine(
				//    "Press ENTER to continue");
				//Console.ReadKey();
				Console.WriteLine(
					"========================================================================================================");

				//var actorResourceParameters = new ActorResourceParameters();
				actorResourceParameters.PageNumber = 1;
				actorResourceParameters.PageSize = 20;

				var getActorFilmListReport = repo.GetActorWithFilmListReport(actorResourceParameters);
				var actorFilmListReportCount = getActorFilmListReport.Count();
				Console.WriteLine("GetActorWithFilmListReport");

				Console.WriteLine(
					"========================================================================================================");
				Console.WriteLine("Page One of Paged List of Actor Films");
				foreach (var listItem in getActorFilmListReport)
				{
					Console.WriteLine($"ACTOR: {listItem.FirstName} {listItem.LastName} {listItem.FilmTitle}");
				}

				//var actorResourceParameters = new ActorResourceParameters();
				actorResourceParameters.PageNumber = 2;
				actorResourceParameters.PageSize = 20;

				getActorFilmListReport = repo.GetActorWithFilmListReport(actorResourceParameters);
				actorFilmListReportCount = getActorFilmListReport.Count();
				Console.WriteLine("GetActorWithFilmListReport");

				Console.WriteLine(
					"========================================================================================================");
				Console.WriteLine("Page two of Paged List of Actor Films");
				foreach (var listItem in getActorFilmListReport)
				{
					Console.WriteLine($"ACTOR: {listItem.FirstName} {listItem.LastName} {listItem.FilmTitle}");
				}

				Console.WriteLine(
					"========================================================================================================");
				//var actorResourceParameters = new ActorResourceParameters();
				actorResourceParameters.PageNumber = 3;
				actorResourceParameters.PageSize = 20;

				getActorFilmListReport = repo.GetActorWithFilmListReport(actorResourceParameters);
				actorFilmListReportCount = getActorFilmListReport.Count();
				Console.WriteLine("GetActorWithFilmListReport");

				Console.WriteLine(
					"========================================================================================================");
				Console.WriteLine("Page three of Paged List of Actor Films");
				foreach (var listItem in getActorFilmListReport)
				{
					Console.WriteLine($"ACTOR: {listItem.FirstName} {listItem.LastName} {listItem.FilmTitle}");
				}

				Console.WriteLine(
					"========================================================================================================");
				Console.WriteLine(
					$"There are tolal of {getActorFilmListReport.TotalPages} pages and has a Total Record Count of {getActorFilmListReport.TotalCount}");
			}
		}

		#endregion Public Methods
	} // END of class Program

	//=========================================================================
} // END of namespace DVDStore.API

//=============================================================================

/*
 *
  *        .              .   *
                .    ( To ALL Visual Studio Users)
    *     '       * ( We have taken over your editor.)
                    ( Your code has been abducted )
        * .    '     ( Just relax and enjoy)
                     ( as we probe your data.)
    ' .                      /  .   *
               .-'~~~~'-.   /
    .      .-~�\__/  \__/`~-.         .
         .-~   (oo)  (oo)    ~-.
        (_____//~~\\//~~\\______)
   _.-~`                         `~-._
  /O=O=O=O=O=O=O=O=O=O=O=O=O=O=O=O=O=O\     *
  \___________________________________/.
    RAG      \x x x x x x x/            `.
     .  *     \x_x_x_x_x_x/.    '  .       .
               `.           `.         .'    `.
    ' .     *                          '       `.

 *
 */