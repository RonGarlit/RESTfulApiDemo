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
// **  FileName: RootController.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  Root Controller used for top level of the API.  Also to
// **  getting the links to navigate the API.
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
// **  includes hyper-links to other resources.
// **
// **  Level 4: Versioning - UNOFFICIAL - Versioning APIs always helps to ensure
// **  backward compatibility of a service while adding new features or updating
// **  existing functionality for new clients
// **
// **  Change History
// **
// **  WHEN         WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2021-01-08   rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

using Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DVDStore.API.Controllers.v1_0
{
    /// <summary>
    ///     Root of the DVD Store API
    /// </summary>
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    public class RootController : Controller
    {
        #region Public Methods

        /// <summary>
        ///     Get the Root of the DVD Store API - Provides links to explore areas of the API.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     Gets the root navigation information for the API when a user
        ///     first hits the API Service.  RESTful HATEOAS is in play here.
        /// </remarks>
        [HttpGet(Name = "GetRoot")]
        [HttpHead(Name = "GetRoot")]
        [Produces("application/json")]
        public IActionResult GetRoot()
        {
            // Create links for root of that API that are provided to a caller of this API.
            var links = new List<LinkDto>();

            // Add link to the Root Controller
            links.Add(
                new LinkDto(Url.Link("GetRoot", new { }),
                    "self",
                    "GET"));

            // Add link to the Actors Controller
            links.Add(
                new LinkDto(Url.Link("GetActors", new { }),
                    "actors",
                    "GET"));

            // Add link to the Films Controller
            links.Add(
                new LinkDto(Url.Link("GetFilms", new { }),
                    "films",
                    "GET"));

            return Ok(links);
        } // END of GetRoot

        //=====================================================================

        /// <summary>
        ///     Get Options allowed for the DVD Store API Controller.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     Returns a response telling user the type of actions allowed on
        ///     this controller
        /// </remarks>
        [HttpOptions]
        public IActionResult GetRootOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,OPTIONS");
            return Ok();
        }

        #endregion Public Methods

        // END of GetRootOptions

        //=====================================================================
    } // END of class RootController

    //=========================================================================
} // END of namespace DVDStore.API.Controllers

//=============================================================================

/******************************************************************************
 *
 *
Star Trek NX Alpha Class

 __,---.__                         ________,-----.__
/_________`-------._           _,-'      ___ ---    `--._
 ,----------------------------<         |   |---     `-._`-._
(======(    ==============   | )  - - - - - - - - - - - - - ->
 `----------------------------'         |___|---       __,--'
   `|     ,-----.----.-----._______________________,--'
    |____/     _|____|_     _|_____|_

 _____________,----------.___
 \     |             |       `-.
 (\    |             |        _/
   `----------.--------,-----'
              |       /
              |   []  |
              |- - - -|
             /        |
             |        |
             |- - - - |
            /          \
 ________,-'            \
 |    __                 `---------._________.-----.__
/,--.   `.------- /  ---------.         |   |----     `--._
 \   \ |________ /  ______     `. _ _ _ |___|----      === `-.
 /   / |         \             ,'       |   |----      ===_,-'
\`--' __,'------- \  ---------'     ____|___|----   __,--'
 |_______                ,---------'         `-----'
         `-.            /
            \          /
             |- - - - |
             |     ,. |
             \     `' |
              |- - - -|
              |   []  |
              |       \
   -----------'--------`-----._
 (/    |                       \
 /_____|______            ___,-'
              `----------'

                           \ .---. /
    .__,                   ,'\ | /`.                   .__,
   _/  \\_____________.---'_\  .  /_`---._____________//  \_
    \__//`----------------\         /----------------'\\__/
    '  `                  /`._____,'\                  '  `
                          `._  |  _,'
                             `-"-'

 *
 *****************************************************************************/