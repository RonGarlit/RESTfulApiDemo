// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: ActorsController.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  Actors Controller Class.  This is the first controller and will be
// **  sprinkled with notes for demonstration and training purposes.  I intend
// **  to train others at work and in user group sessions.
// **
// **  The intent of this project is to get us to the MAX LEVEL of the
// **  Richardson Maturity Model:
// **  Level 1: Resources - To be compliant with level 1, our API must be
// **  using resources that reside at their URLs.
// **
// **  Level 2: HTTP methods - We have to be level 1 compliant, and level 2
// **  also requires us to use correct HTTP methods for operating on the resources.
// **
// **  Level 3: HATEOAS - Hate-what? Hypermedia As The Engine Of Application
// **  State. It's a hipster way of saying that the response from the service
// **  includes hyperlinks to other resources.
// **
// **  Level 4: Versioning - UNOFFICIAL - Versioning APIs always helps to ensure
// **  backward compatibility of a service while adding new features or updating
// **  existing functionality for new clients
// **
// **  Change History
// **
// **  WHEN         WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-11-13   rgarlit     STARTED DEVELOPMENT
// **  2020-11-16   rgarlit     Added AutoMapper.Extensions.Microsoft.DependencyInjection
// **  2021-01-07   rgarlit     Re-worked to revise API Controller design with limited methods
// **  2021-01-16   rgarlit     Decided to add HttpHead to all Get Methods as good practice
// ***********************************************************************************/

using AutoMapper;
using DVDStore.Common.Models.v1_0.Dto;
using DVDStore.DAL.Models;
using DVDStore.DAL.Repositories.v1_0;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DVDStore.API.Areas.Catalog.Controllers.v1_0
{
	/// <summary>
	///     ActorsController
	/// </summary>
	/// <remarks>
	///     I've sprinkled "Notes for Training" through this controller with
	///     links to MSDN documentation and other sources.
	///     ===================================================================
	///     Notes for Training:
	///     Routing to controller actions in ASP.NET Core -
	///     https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.apicontrollerattribute?view=aspnetcore-5.0
	///     Attribute routing for REST APIs -
	///     https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#ar
	///     Controller action return types in ASP.NET Core web API -
	///     https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-5.0
	///     Started Adding HttpHead Verb to all Get methods.  My reasoning is
	///     this. Making API requests with HEAD methods is actually an
	///     effective
	///     way of simply verifying that a resource is available. It is good
	///     practice to have a test for HEAD requests everywhere you have a
	///     test for GET requests (as long as the API supports it).
	///     HttpVerbs - Important Reading:
	///     https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#http-verb-templates
	///     Route Names
	///     https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#route-name
	/// </remarks>
	[ApiVersion("1.0", Deprecated = true)]
	[ApiController]
	[Area("Catalog")]
	[Route("api/v{version:apiVersion}/[controller]")] // Using well designed ROUTES gets us to Richardson Maturity Model Level 1.
	public class ActorsController : ControllerBase
	{
		#region Private Fields

		private readonly IDvdStoreRepository _dvdStoreRepository; // Hold repository being passed in through constructor
		private readonly IMapper _mapper; // Hold AutoMapper object being passed in through constructor

		#endregion Private Fields

		#region Controller Constructor

		/// <summary>
		///     ActorsController Constructor
		/// </summary>
		/// <param name="dvdStoreRepository"></param>
		/// <param name="mapper"></param>
		public ActorsController(IDvdStoreRepository dvdStoreRepository, IMapper mapper)
		{
			// Setup DVDStore Repository being passed in and check for null
			_dvdStoreRepository = dvdStoreRepository ?? throw new ArgumentNullException(nameof(dvdStoreRepository));
			// Set up AutoMapper being passed in and check for null
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
			_mapper = mapper
				?? throw new ArgumentNullException(nameof(_mapper));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
		} // END of ActorsController Constructor

		//=====================================================================

		#endregion Controller Constructor

		#region Controller Methods

		/// <summary>
		///     Delete an Actor by actorId
		/// </summary>
		/// <param name="actorId"></param>
		/// <returns></returns>
		/// <remarks>
		///     This is used to an actor by Id
		/// </remarks>
		[HttpDelete("{actorId}", Name = "DeleteActor")]
		public ActionResult DeleteActor(int actorId)
		{
			// Get the actor we want to delete
			var actorFromRepo = _dvdStoreRepository.GetActorById(actorId);
			// If NULL there wasn't and actor to delete
			if (actorFromRepo == null)
			{
				// Return an HTTP status code 404
				return NotFound();
			}

			// Setup to delete the actor
			_dvdStoreRepository.DeleteActor(actorFromRepo);
			// Actually delete the actor
			_dvdStoreRepository.Save();

			// Return an HTTP status code 204
			return NoContent();
			// Notes for Training: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.nocontent
			// Which returns a HTTP status code 204.
			// A successful response of DELETE requests SHOULD be HTTP response code 200 (OK).
			// If the response includes an entity describing the status.
			//
			// Else a 202 (Accepted) if the action has been queued,
			// or 204 (No Content) if the action has been performed but the
			// response does not include an entity.
		}

		/// <summary>
		///     Get a single Actor by the actorId
		/// </summary>
		/// <param name="actorId"></param>
		/// <returns>
		///     IActionResult -
		///     https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-5.0#iactionresult-type
		/// </returns>
		/// <remarks>
		///     This is used to get and actor by Id
		/// </remarks>
		[HttpGet("{actorId}", Name = "GetActor")]
		[HttpHead("{actorId}", Name = "GetActor")]
		public ActionResult<ActorDto> GetActor(int actorId)
		{
			// Get Author by ID data from the db using the repository
			var actorFromRepo = _dvdStoreRepository.GetActorById(actorId);

			// Make sure it is not null and that we actually got data back
			if (actorFromRepo == null)
			{
				// If not data return a 404 error. See: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404
				return NotFound(); // Return 404 status code
								   // Notes for training: NotFound Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.notfound
			}

			return Ok(_mapper.Map<ActorDto>(actorFromRepo)); // Return data and 200 status code
															 // Notes for Training: Ok Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok
															 // For any given HTTP GET API, if the resource
															 // is found on the server, then it must return HTTP response
															 // code 200 (OK) – along with the response body, which is usually
															 // either XML or JSON content (due to their platform-independent nature).
															 //
															 // In case resource is NOT found on server then it must return
															 // HTTP response code 404 (NOT FOUND). Similarly, if it is
															 // determined that GET request itself is not correctly formed
															 // then server will return HTTP response code 400 (BAD REQUEST).
		}

		/// <summary>
		///     Get ALL the Actors
		/// </summary>
		/// <returns>
		///     ActionResult -
		///     https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-5.0#actionresultt-type
		/// </returns>
		/// <remarks>
		///     This is used to get a list of actors.
		///     TODO: Set this up for paging later using the methods working in
		///     repo
		/// </remarks>
		[HttpGet(Name = "GetActors")]
		[HttpHead(Name = "GetActors")]
		public ActionResult<IEnumerable<ActorDto>> GetActors()
		{
			// Get authors data from the db using the repository
			var actorsFromRepo = _dvdStoreRepository.GetActors();
			// Notes for Training: Ok Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok
			// Use auto mapper to map the repo data to the DTO (Front Facing) model
			return Ok(_mapper.Map<IEnumerable<ActorDto>>(actorsFromRepo));
			// Notes for Training: For any given HTTP GET API, if the resource
			// is found on the server, then it must return HTTP response
			// code 200 (OK) – along with the response body, which is usually
			// either XML or JSON content (due to their platform-independent nature).
			//
			// In case resource is NOT found on server then it must return
			// HTTP response code 404 (NOT FOUND). Similarly, if it is
			// determined that GET request itself is not correctly formed
			// then server will return HTTP response code 400 (BAD REQUEST).
		} // END of GetActors

		//=====================================================================

		// END of GetActor

		//=====================================================================

		/// <summary>
		///     Get the Options allowed for Actors controller
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		///     Returns a response telling user the type of actions allowed on
		///     this controller
		/// </remarks>
		[HttpOptions]
		public IActionResult GetActorsOptions()
		{
			Response.Headers.Add("Allow", "GET,HEAD,OPTIONS,POST,PUT");
			return Ok();
		}

		/// <summary>
		///     Post a new Actor to the DB.
		/// </summary>
		/// <param name="actor"></param>
		/// <returns></returns>
		/// <remarks>
		///     This is used to ADD a new Actor
		/// </remarks>
		[HttpPost(Name = "PostActor")]
		public ActionResult<ActorDto> PostActor(ActorForCreationDto actor)
		{
			// Setup up entity to pass to repo
			var actorEntity = _mapper.Map<Actor>(actor);
			// Pass entity to the repo
			_dvdStoreRepository.AddActor(actorEntity);
			// Issue SAVE command through repo to save new entry to database
			_dvdStoreRepository.Save();
			// Setup the return object after adding that has the new ID created
			// by saving to database
			var actorToReturn = _mapper.Map<ActorDto>(actorEntity);
			//
			return CreatedAtRoute("GetActor", new { actorId = actorToReturn.Actorid }, actorToReturn);
			// Notes for Training: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.createdatrouteresult?view=aspnetcore-5.0
			// For which an ActionResult that returns a Created (201) response
			// with a Location header.
			//
			// Ideally, if a resource has been created on the origin server, the
			// response SHOULD be HTTP response code 201 (Created) and contain
			// an entity which describes the status of the request and refers
			// to the new resource, and a Location header.
			//
			// Many times, the action performed by the POST method might not
			// result in a resource that can be identified by a URI. In this
			// case, either HTTP response code 200 (OK) or 204 (No Content) is
			// the appropriate response status.
			//
			// IMPORTANT: Please note that POST is neither safe nor idempotent,
			// and invoking two identical POST requests will result in two different
			// resources containing the same information (except resource ids).
		} // END of PostActor

		//=====================================================================

		// END of DeleteActor

		//=====================================================================

		/// <summary>
		///     Put/Update a specific Actor by actorId using and Actor Object
		/// </summary>
		/// <param name="actorId"></param>
		/// <param name="actorForUpdate"></param>
		/// <returns></returns>
		/// <remarks>
		///     This is to update an actor or add as new if doesn't exist.
		/// </remarks>
		[HttpPut]
		public IActionResult PutUpdateActor(int actorId, ActorForUpdateDto actorForUpdate)
		{
			// Check the object with updates
			if (actorForUpdate == null)
			{
				return NotFound();
			}

			// Get the actor to update from DB
			var actorFromRepo = _dvdStoreRepository.GetActorById(actorId);
			//-----------------------------------------------------------------
			// If the requested actor to update was not in the database we
			// ADD new here.  This was just one way you would do in the case
			// of say a social media site.  But you would likely keep this
			// process separated for stuff like financial, Medical, etcetera.
			//-----------------------------------------------------------------
			if (actorFromRepo == null)
			{
				var actorToAdd = _mapper.Map<Actor>(actorForUpdate);

				_dvdStoreRepository.AddActor(actorToAdd);
				_dvdStoreRepository.Save();

				var actorToReturn = _mapper.Map<ActorDto>(actorToAdd);

				return CreatedAtRoute("GetActor", new { actorId = actorToReturn.Actorid }, actorToReturn);
				// Notes for Training: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.createdatrouteresult?view=aspnetcore-5.0
				// For which an ActionResult that returns a Created (201) response
				// with a Location header.
				//
				// Ideally, if a resource has been created on the origin server, the
				// response SHOULD be HTTP response code 201 (Created) and contain
				// an entity which describes the status of the request and refers
				// to the new resource, and a Location header.
				//
				// Many times, the action performed by the POST method might not
				// result in a resource that can be identified by a URI. In this
				// case, either HTTP response code 200 (OK) or 204 (No Content) is
				// the appropriate response status.
				//
				// IMPORTANT: Please note that POST is neither safe nor idempotent,
				// and invoking two identical POST requests will result in two different
				// resources containing the same information (except resource ids).
			}
			//-----------------------------------------------------------------

			//*****************************************************************
			// Update the actor here
			//*****************************************************************
			_mapper.Map(actorForUpdate, actorFromRepo);

			_dvdStoreRepository.UpdateActor(actorFromRepo);
			_dvdStoreRepository.Save();

			return NoContent();
			// Notes for Training: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.nocontent
			// Which returns a HTTP status code 204.
			//
			// Use PUT APIs primarily to update existing resource (if the
			// resource does not exist, then API may decide to create a
			// new resource or not). If a new resource has been created by the
			// PUT API, the origin server MUST inform the user agent via the
			// HTTP response code 201 (Created) response and if an existing
			// resource is modified, either the 200 (OK) or 204 (No Content)
			// response codes SHOULD be sent to indicate successful completion
			// of the request.
			//
			// Important: The difference between the POST and PUT APIs can be
			// observed in request URIs. POST requests are made on resource
			// collections, whereas PUT requests are made on a single resource.
		} // END of PutUpdateActor

		//=====================================================================

		// END of GetActorsOptions

		//=====================================================================

		#endregion Controller Methods

		#region Disabled code to moved later

		//[HttpGet("{id}")]
		//[ActionName("GetActorByIdWithFilmListing")]
		//[Route("api/actors/GetActorWithFilms")]
		//public IActionResult GetActorByIdWithFilmList(int id)
		//{
		//    // Get Author by ID data from the db using the repository
		//    var actorFromRepo = _dvdStoreRepository.GetActorByIdWithFilmListing(id);

		//    // Make sure it is not null and that we actually got data back
		//    if (actorFromRepo == null)
		//    {
		//        // If not data return a 404 error. See: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404
		//        return NotFound(); // Return 404 status code
		//        // Notes for training: NotFound Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.notfound
		//    }

		//    var mappedDataFromRepo = _mapper.Map<ActorDto>(actorFromRepo); // Check that AutoMapper Worked

		//    var resultViewModel = new ActorFilmListViewModel
		//    {
		//        Actorid = mappedDataFromRepo.Actorid,
		//        Firstname = mappedDataFromRepo.Firstname,
		//        Lastname = mappedDataFromRepo.Lastname,
		//        FullName = mappedDataFromRepo.FullName,
		//        Lastupdate = mappedDataFromRepo.Lastupdate
		//    };

		//    var filmList = new List<FilmViewModel>();
		//    foreach (var filmactor in mappedDataFromRepo.Filmactors)
		//    {
		//        var tempFilm = new FilmViewModel
		//        {
		//            Filmid = filmactor.Film.Filmid,
		//            Title = filmactor.Film.Title,
		//            Description = filmactor.Film.Description,
		//            Releaseyear = filmactor.Film.Releaseyear,
		//            Languageid = filmactor.Film.Languageid,
		//            Originallanguageid = filmactor.Film.Originallanguageid,
		//            Rentalduration = filmactor.Film.Rentalduration,
		//            Rentalrate = filmactor.Film.Rentalrate,
		//            Length = filmactor.Film.Length,
		//            Replacementcost = filmactor.Film.Replacementcost,
		//            Rating = filmactor.Film.Rating,
		//            Specialfeatures = filmactor.Film.Specialfeatures,
		//            Lastupdate = filmactor.Film.Lastupdate
		//        };

		//        filmList.Add(tempFilm);
		//    }

		//    resultViewModel.ActorFilmList = filmList;

		//    return Ok(resultViewModel); // Return data and 200 status code
		//    // Notes for Training: Ok Method - https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok
		//} // END of GetActorByIdWithFilmList

		#endregion Disabled code to moved later

		//=====================================================================
	} // END of class ActorsController

	//=========================================================================
} // END of namespace DVDStore.API.Controllers

//=============================================================================

/* Just tossing in some ascii art. To keep my fellow geeks entertained  :-)
 *
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