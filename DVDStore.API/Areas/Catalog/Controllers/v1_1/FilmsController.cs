// /********************************************************************************** **
// **  RESTfulApiPrototype v1.1 **
// **  Copyright 2020
// **  Developed by: Ronald A. Garlit . **
// *********************************************************************************** **
// **  FileName: FilmsController.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit **
// **  Description: **
// **  Films Controller Class.
// **  This is sprinkled with notes for demonstration and training purposes.
// **  I intend to train others at work and in user group sessions. **
// **  The intent of this project is to get us to the MAX LEVEL of the
// **  Richardson Maturity Model:
// **  Level 1: Resources - To be compliant with level 1, our API must be
// **  using resources that reside at their URLs. **
// **  Level 2: HTTP methods - We have to be level 1 compliant, and level 2
// **  also requires us to use correct HTTP methods for operating on the resources. **
// **  Level 3: HATEOAS - Hate-what? Hypermedia As The Engine Of Application
// **  State. It's a hipster way of saying that the response from the service
// **  includes hyperlinks to other resources. **
// **  Level 4: Versioning - UNOFFICIAL - Versioning APIs always helps to ensure
// **  backward compatibility of a service while adding new features or updating
// **  existing functionality for new clients **
// **  Change History **
// **  WHEN WHO WHAT **---------------------------------------------------------------------------------
// **  2020-11-20 rgarlit STARTED DEVELOPMENT
// **  2021-01-16 rgarlit Decided to add HttpHead to all Get Methods as good practice
// **  2021-01-16 rgarlit Clean up Routes to rely more on HttpVerbs with
// **                           route template and naming there.
// **  2021-01-18 rgarlit Working on RESTful API Level 4 Versioning in place
// **  2021-02-15 rgarlit Working on RESTful API Level 3 stuff
// **  2021-02-18 rgarlit Working on Data Shaping stuff
// ***********************************************************************************/

using AutoMapper;
using DVDStore.Common.Models.v1_1.Dto;
using DVDStore.DAL.Repositories.v1_1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DVDStore.API.Areas.Catalog.Controllers.v1_1
{
	/// <summary>
	/// FilmsController
	/// </summary>
	/// <remarks>
	/// I've sprinkled "Notes for Training" through this controller with links to MSDN documentation
	/// and other sources. =================================================================== Notes
	/// for Training: Routing to controller actions in ASP.NET Core -
	/// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.apicontrollerattribute?view=aspnetcore-5.0
	/// Attribute routing for REST APIs -
	/// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#ar
	/// Controller action return types in ASP.NET Core web API -
	/// https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-5.0
	/// Started Adding HttpHead Verb to all Get methods. My reasoning is this. Making API requests
	/// with HEAD methods is actually an effective way of simply verifying that a resource is
	/// available. It is good practice to have a test for HEAD requests everywhere you have a test
	/// for GET requests (as long as the API supports it). HttpVerbs - Important Reading:
	/// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#http-verb-templates
	/// Route Names https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#route-name
	/// </remarks>
	[ApiVersion("1.1")]
	[ApiController]
	[Area("Catalog")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class FilmsController : Controller
	{
		#region Private Fields

		private readonly IDvdStoreRepository _dvdStoreRepository; // Hold repository being passed in through constructor
		private readonly IMapper _mapper;

		#endregion Private Fields

		// Hold AutoMapper object being passed in through constructor

		#region Public Constructors

		/// <summary>
		/// FilmsController Constructor
		/// </summary>
		/// <param name="dvdStoreRepository"> </param>
		/// <param name="mapper"> </param>
		public FilmsController(IDvdStoreRepository dvdStoreRepository, IMapper mapper)
		{
			// Setup DVDStore Repository being passed in and check for null
			_dvdStoreRepository = dvdStoreRepository ?? throw new ArgumentNullException(nameof(dvdStoreRepository));
			// Set up AutoMapper being passed in and check for null
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
			_mapper = mapper
				?? throw new ArgumentNullException(nameof(_mapper));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
		}

		#endregion Public Constructors

		// END of ActorsController Constructor

		//=====================================================================

		#region Public Methods

		/// <summary>
		/// Get a specific Film for and Actor by actorId and filmId
		/// </summary>
		/// <param name="actorId"> </param>
		/// <param name="filmId"> </param>
		/// <returns> FilmDto object </returns>
		[HttpGet("{actorId}/{filmId}", Name = "GetFilmForActor")]
		[HttpHead("{actorId}/{filmId}", Name = "GetFilmForActor")]
		public ActionResult<FilmDto> GetFilm(int actorId, int filmId)
		{
			// Check for actor first
			if (!_dvdStoreRepository.ActorExists(actorId))
			{
				return NotFound();
			}

			// Get film data for the actor from the db using the repository
			var filmFromRepo = _dvdStoreRepository.GetFilm(actorId, filmId);
			// Check if that film exists for the actor
			if (filmFromRepo == null)
			{
				return NoContent();
			}

			// Use auto mapper to map the repo data to the DTO (Front Facing) model
			return Ok(_mapper.Map<FilmDto>(filmFromRepo));
			// Notes for Training: Ok Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok
		}

		/// <summary>
		/// Get all the Films
		/// </summary>
		/// <returns> IEnumerable List of FilmDto </returns>
		[HttpGet(Name = "GetFilms")]
		[HttpHead(Name = "GetFilms")]
		public ActionResult<IEnumerable<FilmDto>> GetFilms()
		{
			// Get film data from the db using the repository
			var filmsFromRepo = _dvdStoreRepository.GetFilms();
			// Use auto mapper to map the repo data to the DTO (Front Facing) model
			return Ok(_mapper.Map<IEnumerable<FilmDto>>(filmsFromRepo));
			// Notes for Training: Ok Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok
		} // END of ActionResult<IEnumerable<FilmDto>> GetFilms()

		//=====================================================================

		/// <summary>
		/// Get all the Films for a specific Actor by actorId
		/// </summary>
		/// <param name="actorId"> </param>
		/// <returns> IEnumerable List of FilmDto </returns>
		[HttpGet("{actorId}", Name = "GetFilmsForActor")]
		[HttpHead("{actorId}", Name = "GetFilmsForActor")]
		public ActionResult<IEnumerable<FilmDto>> GetFilms(int actorId)
		{
			// Check for actor first
			if (!_dvdStoreRepository.ActorExists(actorId))
			{
				return NotFound();
			}

			// Get films for the actor from the db using the repository
			var filmsFromRepo = _dvdStoreRepository.GetFilms(actorId);
			// Use auto mapper to map the repo data to the DTO (Front Facing) model
			return Ok(_mapper.Map<IEnumerable<FilmDto>>(filmsFromRepo));
			// Notes for Training: Ok Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok
		} // END of ActionResult<IEnumerable<FilmDto>> GetFilms(int actorId)

		//=====================================================================

		// END of ActionResult<FilmDto> GetFilm(int actorId, int filmId)

		//=====================================================================

		/// <summary>
		/// Get the Options allowed for the Films controller
		/// </summary>
		/// <returns> </returns>
		/// <remarks> Returns a response telling user the type of actions allowed on this controller </remarks>
		[HttpOptions]
		public IActionResult GetFilmsOptions()
		{
			Response.Headers.Add("Allow", "GET,HEAD,OPTIONS");
			return Ok();
		}

		#endregion Public Methods

		// END of GetFilmsOptions

		//=====================================================================
	} // END of class FilmsController

	//=========================================================================
} // END of namespace DVDStore.API.Controllers

//=============================================================================

/*

                 _,--=--._
               ,'    _    `.
              -    _(_)_o   -
         ____'    /_  _/]    `____
  -=====::(+):::::::::::::::::(+)::=====-
           (+).""""""""""""",(+)
               .           ,
                 `  -=-  '

                                          _____________
                                       __/_|_|_|_|_|_|_\__
                                      /                   \    .
                 .       ____________/  ____               \   :
                 :    __/_|_|_|_|_|_(  |    |               )  |
                 |   /               \ | () |()  ()  ()  ()/   *
                 *  /  ____           \|____|_____________/
    .              (  |    |            \_______________/
    :               \ | () |()  ()  ()    \___________/
    |                \|____|____________ /   \______/     .
    *                  \_______________/       \  /       :
          3         .    \___________/         (__)       |    .
            3       :       \______/           /  \       *    :
             3      |         \  /            /    \           |
              3     *         (__)           /      \          *
        ,,     3              /  \          /        \
      w`\v',___n___          /    \        /          \
      v\`|Y/      /\        /      \      /            \
      `-Y/-'_____/  \      /        \    /              \
       `|-'      |  |     /          \  /                \
________|_|______|__|____/____________\/__________________\__

RAG
 */