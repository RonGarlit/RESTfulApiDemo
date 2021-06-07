// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2021
// **  Developed by:  Ronald A. Garlit .
// **
// **  This software is the proprietary information of 21st Century Oncology.
// **
// **  Use is subject to license terms.
// ***********************************************************************************
// **
// **  FileName: ConfigureSwaggerOptions.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  ConfigureSwaggerOptions class used for Swagger setup and tweaked to
// **  handle API Versioning.
// **
// **  Change History
// **
// **  WHEN        WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2021-01-18  rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DVDStore.API
{
	/// <summary>
	///     Configures the Swagger generation options.
	/// </summary>
	/// <remarks>
	///     <para>
	///         This allows API versioning to define a Swagger document per
	///         API version after the
	///         <see cref="IApiVersionDescriptionProvider" /> service has
	///         been resolved from the service container.
	///     </para>
	///     <para>
	///         Taken from
	///         https://github.com/microsoft/aspnet-api-versioning.
	///     </para>
	/// </remarks>
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		#region Private Fields

		private readonly IApiVersionDescriptionProvider _provider;

		#endregion Private Fields

		#region constructors and destructors

		/// <summary>
		///     Initializes a new instance of the
		///     <see cref="ConfigureSwaggerOptions" /> class.
		/// </summary>
		/// <param name="provider">
		///     The <see cref="IApiVersionDescriptionProvider">provider</see>
		///     used to generate Swagger
		///     documents.
		/// </param>
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
		{
			_provider = provider;
		}

		#endregion constructors and destructors

		#region explicit interfaces

		/// <inheritdoc />
		public void Configure(SwaggerGenOptions options)
		{
			// add a swagger document for each discovered API version
			// note: you might choose to skip or document deprecated API versions differently
			foreach (var description in _provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
			}
		}

		#endregion explicit interfaces

		#region methods

		/// <summary>
		///     Internal implementation for building the Swagger basic config.
		/// </summary>
		/// <param name="description">The description object containing the.</param>
		/// <returns>The generated Open API info.</returns>
		private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
		{
			var info = new OpenApiInfo
			{
				Title = "DVDStore.API",
				Version = description.ApiVersion.ToString(),
				Description = @"<p>DVDStore.API with versioning setup. Choose API version using the <strong> ""Select a definition"" </strong> selector on the above right.</p>",
				Contact = new OpenApiContact
				{
					Name = "Ronald Garlit",
					Email = "RonGarlit@live.com"
				}
			};
			if (description.IsDeprecated)
			{
				OpenApiLicense tempLic = new OpenApiLicense
				{
					Name = @"THIS VERSION IS DEPRECATED"
				};
				info.License = tempLic;

				//var depMsg = @"<strong><div>THIS VERSION IS DEPRECATED</div></strong>";
				//info.Description += depMsg;
			}

			return info;
		}

		#endregion methods
	} // END of class ConfigureSwaggerOptions

	//=========================================================================
} // END of namespace DVDStore.API

//=============================================================================