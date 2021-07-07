// /********************************************************************************** **
// **  RESTfulApiPrototype v1.0 **
// **  Copyright 2020
// **  Developed by: Ronald A. Garlit . **
// *********************************************************************************** **
// **  FileName: Startup.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit **
// **  Description: Program Description **
// **  Main Startup class for configuration of the application. **
// **  Change History **
// **  WHEN WHO WHAT **---------------------------------------------------------------------------------
// **  2020-10-28 rgarlit STARTED DEVELOPMENT Rewrite of API Prototype
// **  2020-10-29 rgarlit Replaced .NET Core internal logging with NLog
// **  2020-11-09 rgarlit Tied in Entity Framework Logging into NLog
// **  2020-11-22 rgarlit OpenAPI and Swagger UI in ASP.NET Core 5 Web
// **                     API Explained: https://www.davidhayden.me/blog/openapi-and-swagger-ui-in-asp-net-core-5-web-api
// **  2021-01-18 rgarlit Working on RESTful API Level 4 Versioning in place
// **  2021-02-15 rgarlit Working on RESTful API Level 3 stuff
// **  2021-02-18 rgarlit Working on Data Shaping stuff
// ***********************************************************************************/

using DVDStore.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace DVDStore.API
{
	/// <summary>
	/// Startup
	/// </summary>
	/// <remarks>
	/// I've sprinkled "Notes for Training" through this controller with links to MSDN documentation
	/// Notes for Training: Start up class in .NET 5
	/// </remarks>
	public class Startup
	{
#if EFCoreLogging

		#region Public Fields

		/// <summary>
		/// EfNLogLoggerFactory
		/// </summary>
		/// <remarks> Used to tie Entity Framework Core logging into NLog </remarks>
		public static readonly LoggerFactory EfNLogLoggerFactory
			= new LoggerFactory(new[] { new NLogLoggerProvider() });

		#endregion Public Fields

#endif

		#region Public Constructors

		/// <summary>
		/// Startup Constructor
		/// </summary>
		/// <param name="configuration"> </param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			//Program.Configuration = configuration;
		}

		#endregion Public Constructors

		// END of Startup Constructor

		//=====================================================================

		#region Internal Properties

		// Changed my way of getting configuration available Here I'm making using and internal
		// static variable so it is just called simply using: Startup.Configuration["MyApplication:Name"]
		internal static IConfiguration Configuration { get; set; }

		#endregion Internal Properties

		// Configuration Property (Part of another way)
		//public IConfiguration Configuration { get; }
		//=====================================================================

		#region Public Methods

		/// <summary>
		/// Configure
		/// </summary>
		/// <param name="app"> </param>
		/// <param name="env"> </param>
		/// <param name="provider"> </param>
		/// <remarks>
		/// This method gets called by the runtime. Use this method to configure the HTTP request
		/// pipeline. 2021-01-18 - RG - Added to Configure the IApiVersionDescriptionProvider provider
		/// </remarks>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			//logger.LogInformation("Logged using LogInformation in the Configure method of Startup");
			//
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				// Add Swashbuckle (Swagger API test page)
				// for this application you can call in the browser with http://localhost:51000/swagger/index.html
				//app.UseSwagger();
				//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DVDStore.API v1"));
			}
			else
			{
				// FYI: May need to log these errors -RG
				app.UseExceptionHandler(appBuilder =>
				{
					appBuilder.Run(async context =>
					{
						context.Response.StatusCode = 500;
						await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
					});
				});
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			// 2021-01-18 - RG - Change placement of SwashBuckle code here: Add Swashbuckle (Swagger
			// API test page) for this application you can call in the browser with http://localhost:51000/swagger/index.html
			app.UseSwagger();
			//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DVDStore.API v1")); // Without Versioning stuff
			app.UseSwaggerUI(
				options =>
				{
					// build a swagger endpoint for each discovered API version
					foreach (var description in provider.ApiVersionDescriptions)
					{
						options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
							description.GroupName.ToUpperInvariant());
					}
				});

			//logger.LogInformation("Exiting the Configure method of Startup");
		}

		/// <summary>
		/// ConfigureServices
		/// </summary>
		/// <param name="services"> </param>
		/// <remarks>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </remarks>
		public void ConfigureServices(IServiceCollection services)
		{
			//-----------------------------------------------------------------
			// This adds about everything we need for doing and API without
			// all the junk from adding the full MVC stuff
			// This is where you setup for stuff for better control like
			// certain actions and output formatters
			//-----------------------------------------------------------------
			services.AddControllers(setupAction =>
				{
					//setupAction.ReturnHttpNotAcceptable =
					//    true; // will return 406 Not Acceptable if they ask for XML and no formatter setup

					/* FYI: RG-2021-01-06 *************************************
                     * FYI: I reset this to FALSE which is the default because
                     * initially set it to TRUE during testing but caused errors
                     * during post test/plain formating in CreatedAtRoute calls
                     *********************************************************/
					setupAction.ReturnHttpNotAcceptable =
						true; // True will return 406 Not Acceptable if they ask for XML and no formatter setup
							  // For ReturnHttpNotAcceptable: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.mvcoptions.returnhttpnotacceptable
				})
				//.AddNewtonsoftJson(options =>
				//    options.SerializerSettings.ReferenceLoopHandling =
				//        ReferenceLoopHandling.Ignore) // Converted from System.Text.Json to Newtonsoft.Json
				// See https://www.thecodebuzz.com/jsonexception-possible-object-cycle-detected-object-depth/
				// WARNING: For complex EF nested loop-able data models pay
				// attention to this as these new settings get you and
				// infinite loop with the Newtonsoft.Json with the
				// SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				// See:  https://dotnetcoretutorials.com/2020/03/15/fixing-json-self-referencing-loop-exceptions/#:~:text=JsonException%3A%20A%20possible%20object%20cycle%20was%20detected%20which,%3A%20JsonSerializationException%3A%20Self%20referencing%20loop%20detected%20with%20type
				.AddXmlDataContractSerializerFormatters() // Adds the XML DataContractSerializer formatters to API as additional output type.
				.AddXmlSerializerFormatters() //; // Adds the XML Serializer formatters to API as additional output type.
											  // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvcxmlmvcbuilderextensions.addxmldatacontractserializerformatters

			#region Modify Problem Behavior as per Kevin Dockx

				.ConfigureApiBehaviorOptions(setupAction =>
				{
					setupAction.InvalidModelStateResponseFactory = context =>
					{
						// create a problem details object
						var problemDetailsFactory = context.HttpContext.RequestServices
							.GetRequiredService<ProblemDetailsFactory>();
						var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
							context.HttpContext,
							context.ModelState);

						// add additional info not added by default
						problemDetails.Detail = "See the errors field for details.";
						problemDetails.Instance = context.HttpContext.Request.Path;

						// find out which status code to use
						var actionExecutingContext =
							context as ActionExecutingContext;

						// if there are modelstate errors & all keys were correctly found/parsed
						// we're dealing with validation errors
						if (context.ModelState.ErrorCount > 0 &&
							actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count)
						{
							problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
							problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
							problemDetails.Title = "One or more validation errors occurred.";

							return new UnprocessableEntityObjectResult(problemDetails)
							{
								ContentTypes = { "application/problem+json" }
							};
						}

						// if one of the keys wasn't correctly found / couldn't be parsed we're
						// dealing with null/unparsable input
						problemDetails.Status = StatusCodes.Status400BadRequest;
						problemDetails.Title = "One or more errors on input occurred.";
						return new BadRequestObjectResult(problemDetails)
						{
							ContentTypes = { "application/problem+json" }
						};
					};
				})

			#endregion Modify Problem Behavior as per Kevin Dockx

				; // End services.AddControllers Configuration stuff here- RG
				  //-----------------------------------------------------------------

			//-----------------------------------------------------------------
			// API Versioning stuff
			services.AddApiVersioning(
				options =>
				{
					// reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
					options.ReportApiVersions = true;
				});
			services.AddVersionedApiExplorer(
				options =>
				{
					// add the versioned api explorer, which also adds
					// IApiVersionDescriptionProvider service
					// note: the specified format code will format the version as "'v'major[.minor][-status]"
					options.GroupNameFormat = "'v'VVV";
					// note: this option is only necessary when versioning by url segment. the SubstitutionFormat
					// can also be used to control the format of the API version in route templates
					options.SubstituteApiVersionInUrl = true;
				});
			//-------------------------------------------------------------

			// add AutoMapper.Extensions.Microsoft.DependencyInjection Set it to look in all
			// assemblies of the current domain of the application
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			// Added Open API SwashBuckle/Swagger
			services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			services.AddSwaggerGen(c =>
			{
				// IF YOU GET ERROR HERE!!
				// Comment this out if you do not want to add XML Documentation Help File.
				// This is set on the DVDStore.API build property page under the Output section.
				c.IncludeXmlComments(XmlCommentsFilePath);
				//c.SwaggerDoc("v1", new OpenApiInfo {Title = "DVDStore.API", Version = "v1"});
			});

			// Register the repository class for the DVD Store V1_0 Repository Class
			services.AddTransient<Common.PropertyMapping.v1_0.IDvdStorePropertyMapper, Common.PropertyMapping.v1_0.DvdStorePropertyMapper>();
			services.AddTransient<DAL.Repositories.v1_0.IDvdStoreRepository, DAL.Repositories.v1_0.DvdStoreRepository>();

			//V1_1 Repository Class
			services.AddTransient<Common.PropertyMapping.v1_1.IDvdStorePropertyMapper, Common.PropertyMapping.v1_1.DvdStorePropertyMapper>();
			services.AddTransient<DAL.Repositories.v1_1.IDvdStoreRepository, DAL.Repositories.v1_1.DvdStoreRepository>();

			// ApiHelpers Used throughout - Helper thus not version dependent
			services.AddTransient<Api.Helpers.PropMapHelpers.IPropertyCheckerService, Api.Helpers.PropMapHelpers.PropertyCheckerService>();

			// Register the DbContext class for the DVD Store
			services.AddTransient<IDVDStoreDBContext, DVDStoreDBContext>();

			// Register our DVDStoreDBContext from the DAL project Notes for Training: DbContext in
			// dependency injection for ASP.NET Core https://docs.microsoft.com/en-us/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core
			services.AddDbContext<DVDStoreDBContext>(options =>
			{
#if EFCoreLogging
				// Setup EF Logging - This will allow you to see things like the queries being
				// generated by EF Core and other debug and info logging code from EF.
				options.UseLoggerFactory(EfNLogLoggerFactory); // Tie EF Core Logging into NLOG
				options.EnableSensitiveDataLogging(); // So you can see the EF SQL Parameter values
#endif
				string connString = Configuration["ConnectionStrings:DVDStoreDb"];
				options.UseSqlServer(connString);
			}); // END of AddDbCopntext and Options it is configured for.
		}

		#endregion Public Methods

		// END of ConfigureServices

		//=====================================================================

		//END of Configure

		//=====================================================================

		#region properties

		/// <summary>
		/// Retrieves the absolute file path of the XML documentation file.
		/// </summary>
		private static string XmlCommentsFilePath
		{
			get
			{
				var basePath = PlatformServices.Default.Application.ApplicationBasePath;
				var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
				return Path.Combine(basePath, fileName);
			}
		}

		#endregion properties
	} // END of class Startup
	  
	//=========================================================================
} // END of namespace DVDStore.API

//=============================================================================

/*
 *

                 _                ( Hey down there! Don't )
                /\\               ( you blockheads know )
                \ \\  \__/ \__/  / ( that these things )
                 \ \\ (oo) (oo) /    ( CAN'T crash!! )
                  \_\\/~~\_/~~\_
                 _.-~===========~-._
                (___/_______________)
                   /  \_______/
       ( What a bunch of nuts! )

            _                                      _
      __   |.|       ___________                  | \
     / .|  | |      /  .  > .   \                /   \
    |.' |  |'|     / '  ..  "    \____          |.' ~|
____|  .|__| |____/ .  _________ '    \____..---| .  |__..------_________
RAG .   _________     | ROSWELL|   __________   __________  '  |This was |
 .'     |50 years|  ' |HAPPENED|  |Secret UFO| | MAKE THE |  . |the crash|
        |of cover|    |________|  |Bodies!!! | |GOVERNMENT|    |__SITE!__|
        |__up!!__|        ||  " . |__________| |_REVEAL!!_|        ||
  '   _____ ||       @@@@@||          ||           ||              ||
     //ovo\\||   .. @@*.*@@|     )))) |m .    \\\\ |m       //"\\  ||
      \_-_/ m|       @\'/@/|    (~OO~)//      /^v^\|\\  .  //ovo\\ |m
     //\_/\//| .  '  /( )/       \--///       \ o /_//      \ ~ /  //
    //|   |/        //) (  . '  /|  |         /             //  \\//
 *
 */