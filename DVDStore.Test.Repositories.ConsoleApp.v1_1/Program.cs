// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: Program.cs (DVDStore.Test.Repositories.ConsoleApp)
// **  Version: 0.1
// **  Author: Ronald A. Garlit 
// **
// **  Description: 
// **
// **  Test Repository class in Console application because EF Core Unit
// **  Testing Debugging having an issue in VS2019.
// **
// **  Change History
// **
// **  WHEN        WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-11-06  rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

using System;
using System.IO;
using System.Linq;
using DVDStore.Common.Models.v1_1.Dto;
using DVDStore.Common.PropertyMapping.v1_1;
using DVDStore.Common.ResourceParameters.v1_1;
using DVDStore.DAL.Context;
using DVDStore.DAL.Models;
using DVDStore.DAL.Repositories.v1_1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace DVDStore.Test.Repositories.ConsoleApp.v1_1
{
    internal class Program
    {

#if EFCoreLogging
        /// <summary>
        ///     EfNLogLoggerFactory
        /// </summary
        /// <remarks>
        ///     Used to tie Entity Framework Core logging into NLog
        /// </remarks>
        public static readonly LoggerFactory EfNLogLoggerFactory
            = new LoggerFactory(new[] { new NLogLoggerProvider() });
#endif
        private static void Main(string[] args)
        {
            //=================================================================
            // Setup the configuration so you can get items in the AppSettings.json file
            //=================================================================
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            //=================================================================
            // Get a connectionString from the AppSettings.json file
            //=================================================================
            var connectionString = configuration.GetConnectionString("DVDStoreDb");

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

            // Revised DbContext to accept a ConnectionString in
            // an additional constructor
            var dvdDbRevised = new DVDStoreDBContextRevised(configuration.GetConnectionString("DVDStoreDb"));

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory
                    .GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                .Build();

            // Build out Dependency Injection for application classes
            var servicesProvider = BuildDi(config);


            // Set console colors through out using these commands
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine(
                "Test of EF Core/.NET 5.0 - Used to generate code from the DVD Store (Sakila) Sample Database...");
            Console.WriteLine(
                "========================================================================================================\n\n");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine(
                "NOTE: I had to hand code for stored procedures as that hasn't been built onto dotnet ef command.");
            Console.WriteLine(
                "      This requires hand creation of a model and adding it as entity in the OnModelCreating of DbContext");
            Console.WriteLine("      using Fluent method with the ModelBuilder.");
            Console.WriteLine(
                "========================================================================================================\n\n");


            // Test GetActors() on Repository Class
            Test_Repository_GetActors(dvdDb);

            Test_Repository_GetActorsPageListing(dvdDb);

            Test_Repository_GetActorFilmListById(dvdDb);

            Test_Repository_GetActorsPageListing(dvdDb);

            Test_Repository_GetActorWithFilmListReport(dvdDb);


            // Test the DAL DbContext
            TestDvdStoreDbContext(dvdDb);

            // Test the DAL Revised DbContext for ConnectString in Constructor
            Test_DvdStoreDbContextRevised(dvdDbRevised);

            // Restore the original console colors.
            Console.ResetColor();
        } // END of static void Main(string[] args)
        //=====================================================================

        public static void Test_Repository_GetActorFilmListById(DVDStoreDBContext dvdDb)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine("Test_Repository_GetActorFilmListById");

            using (var repo = new DvdStoreRepository(dvdDb))
            {
                var actorId = 2; //NICK WAHLBERG from test db
                var getActorList = repo.GetActorFilmListById(actorId);

                Console.WriteLine(
                    "========================================================================================================");
                foreach (var actorFilmListItem in getActorList)
                {
                    Console.WriteLine($"{actorFilmListItem.FirstName} {actorFilmListItem.LastName} {actorFilmListItem.FilmId} {actorFilmListItem.FilmTitle} {actorFilmListItem.Rating} ");
                }
            }
        }
        private static void Test_Repository_GetActorsPageListing(DVDStoreDBContext dvdDb)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine("Test_Repository_GetActorsPageListing");
            Console.WriteLine("GetActors(actorResourceParameters)");

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
                Console.WriteLine(
                    "Press ENTER to continue");
                Console.ReadKey();
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Begin Page One...");
                foreach (var actor in getActorList)
                {
                    Console.WriteLine($"ACTOR: {actor.Firstname} {actor.Lastname}");
                }

                //var actorResourceParameters = new ActorResourceParameters();
                actorResourceParameters.PageNumber = 3;
                actorResourceParameters.PageSize = 20;


                getActorList.Clear();
                getActorList = repo.GetActors(actorResourceParameters);
                actorCount = getActorList.Count();
                Console.WriteLine($"Actor Table Record Count:  {actorCount}");
                Console.WriteLine(
                    "Press ENTER to continue");
                Console.ReadKey();
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Jump AHEAD to Page Three...");
                foreach (var actor in getActorList)
                {
                    Console.WriteLine($"ACTOR: {actor.Firstname} {actor.Lastname}");
                }

                Console.WriteLine(
                    "========================================================================================================");
                //var actorResourceParameters = new ActorResourceParameters();
                actorResourceParameters.PageNumber = 2;
                actorResourceParameters.PageSize = 20;


                getActorList.Clear();
                getActorList = repo.GetActors(actorResourceParameters);
                actorCount = getActorList.Count();
                Console.WriteLine($"Actor Table Record Count:  {actorCount}");
                Console.WriteLine(
                    "Press ENTER to continue");
                Console.ReadKey();
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Jump Back to Page Two...");
                foreach (var actor in getActorList)
                {
                    Console.WriteLine($"ACTOR: {actor.Firstname} {actor.Lastname}");
                }

                Console.WriteLine(
                    "========================================================================================================");
                //var actorResourceParameters = new ActorResourceParameters();
                actorResourceParameters.PageNumber = 1;
                actorResourceParameters.PageSize = 20;

                var getActorFilmListReport = repo.GetActorWithFilmListReport(actorResourceParameters);
                var actorFilmListReportCount = getActorFilmListReport.Count();
                Console.WriteLine($"GetActorWithFilmListReport");

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
                Console.WriteLine($"GetActorWithFilmListReport");

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
                Console.WriteLine($"GetActorWithFilmListReport");

                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Page three of Paged List of Actor Films");
                foreach (var listItem in getActorFilmListReport)
                {
                    Console.WriteLine($"ACTOR: {listItem.FirstName} {listItem.LastName} {listItem.FilmTitle}");
                }
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine($"There are tolal of {getActorFilmListReport.TotalPages} pages and has a Total Record Count of {getActorFilmListReport.TotalCount}");
            }
        }

        private static void Test_Repository_GetActorWithFilmListReport(DVDStoreDBContext dvdDb)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine("Test_Repository_GetActorWithFilmListReport");
            Console.WriteLine("GetActors(actorResourceParameters)");

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
                Console.WriteLine($"GetActorWithFilmListReport");

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
                Console.WriteLine($"GetActorWithFilmListReport");

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
                Console.WriteLine($"GetActorWithFilmListReport");

                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Page three of Paged List of Actor Films");
                foreach (var listItem in getActorFilmListReport)
                {
                    Console.WriteLine($"ACTOR: {listItem.FirstName} {listItem.LastName} {listItem.FilmTitle}");
                }
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine($"There are tolal of {getActorFilmListReport.TotalPages} pages and has a Total Record Count of {getActorFilmListReport.TotalCount}");
            }
        } // END of Test_Repository_GetActorWithFilmListReport
        //=====================================================================


        private static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
                //.AddTransient<Runner>() // Runner is the custom class
                .AddSingleton<Actor>()
                .AddSingleton<ActorDto>()
                .AddTransient<IDvdStorePropertyMapper, DvdStorePropertyMapper>()
                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                })
                .BuildServiceProvider();
        }

        private static void Test_Repository_GetActors(DVDStoreDBContext dvdDb)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine("Test_Repository_GetActors");

            using (var repo = new DvdStoreRepository(dvdDb))
            {
                var getActorList = repo.GetActors();
                var actorCount = getActorList.Count();
                Console.WriteLine($"Actor Table Record Count:  {actorCount}");
                Console.WriteLine(
                    "Press ENTER to continue");
                Console.ReadKey();
                Console.WriteLine(
                    "========================================================================================================");
                foreach (var actor in getActorList)
                {
                    Console.WriteLine($"ACTOR: {actor.Firstname} {actor.Lastname}");
                }
            }
        }


        private static void TestDvdStoreDbContext(DVDStoreDBContext dvdDb)
        {
            using (dvdDb)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("TestDvdStoreDbContext");
                Console.WriteLine("Normal DbContext LINQ call for data from a table count");
                var actorRecordCount = dvdDb.Actors.ToList().Count;
                Console.WriteLine($"Actor Table Record Count:  {actorRecordCount}");
                Console.WriteLine(
                    "========================================================================================================");


                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatistic() \n");

                var getDvdStoreDatabaseStatistics = dvdDb.UspGetDatabaseStatistics();

                foreach (var storeDbStat in getDvdStoreDatabaseStatistics)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatisticsAsync() \n");
                var getDvdStoreDatabaseStatisticsAsync = dvdDb.UspGetDatabaseStatisticsAsync();

                foreach (var storeDbStat in getDvdStoreDatabaseStatisticsAsync.Result)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategory() \n");

                var getDvdStorepGetListOfTitlesByCategory = dvdDb.UspGetListOfTitlesByCategory("Sci-Fi");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategory)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategoryAsync() \n");
                var getDvdStorepGetListOfTitlesByCategoryAsync = dvdDb.UspGetListOfTitlesByCategoryAsync("Documentary");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategoryAsync.Result)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");
            } // END of using (dvdDb)
        }

        private static void Test_DvdStoreDbContextRevised(DVDStoreDBContextRevised dvdDbRevised)
        {
            using (dvdDbRevised)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Test_DvdStoreDbContextRevised");
                Console.WriteLine("Normal DbContext LINQ call for data from a table count");
                var paymentRecordCount = dvdDbRevised.Payments.ToList().Count;
                Console.WriteLine($"Payment Table Record Count:  {paymentRecordCount}");
                Console.WriteLine(
                    "========================================================================================================");


                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatistic() \n");

                var getDvdStoreDatabaseStatistics = dvdDbRevised.UspGetDatabaseStatistics();

                foreach (var storeDbStat in getDvdStoreDatabaseStatistics)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatisticsAsync() \n");
                var getDvdStoreDatabaseStatisticsAsync = dvdDbRevised.UspGetDatabaseStatisticsAsync();

                foreach (var storeDbStat in getDvdStoreDatabaseStatisticsAsync.Result)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategory() \n");

                var getDvdStorepGetListOfTitlesByCategory = dvdDbRevised.UspGetListOfTitlesByCategory("Classics");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategory)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategoryAsync() \n");
                var getDvdStorepGetListOfTitlesByCategoryAsync =
                    dvdDbRevised.UspGetListOfTitlesByCategoryAsync("Travel");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategoryAsync.Result)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");



            } // END of using (dvdDbRevised)
        }
    } // END of Program Class

    //=========================================================================
} // END of Namespace
  //=============================================================================