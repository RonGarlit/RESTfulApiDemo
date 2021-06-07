// /********************************************************************************** **
// **  RESTfulApiPrototype v1.1 **
// **  Copyright 2020
// **  Developed by: Ronald A. Garlit . **
// *********************************************************************************** **
// **  FileName: DvdStoreRepository.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit **
// **  Description: **
// **  Main DVD Store Repository Class. **
// **  Change History **
// **  WHEN WHO WHAT 
// **---------------------------------------------------------------------------------
// **  2020-10-27 rgarlit STARTED DEVELOPMENT
// **  2021-01-18 rgarlit Working on RESTful API Level 4 Versioning in place
// **  2021-02-15 rgarlit Working on RESTful API Level 3 stuff
// **  2021-02-18 rgarlit Working on Data Shaping stuff
// ***********************************************************************************/

using Api.Helpers;
using DVDStore.Common.Models.v1_1;
using DVDStore.Common.Models.v1_1.Dto;
using DVDStore.Common.PropertyMapping.v1_1;
using DVDStore.Common.ResourceParameters.v1_1;
using DVDStore.DAL.Context;
using DVDStore.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DVDStore.DAL.Repositories.v1_1
{
    /// <summary>
    /// DvdStoreRepository
    /// </summary>
    public class DvdStoreRepository : IDvdStoreRepository, IDisposable
    {
        #region Private Fields

        // Holds EF DbContext object for the repository
        private readonly IDVDStoreDBContext _dvdStoreDbContext;

        // Holds the Property Mapper object for the repository items generating
        // PagedList object in conjunction with passed in specific object
        // "Resource Parameters"
        private readonly IDvdStorePropertyMapper _dvdStorePropertyMapper;

        #endregion Private Fields

        #region Constructor Methods

        /// <summary>
        /// DvdStoreRepository Constructor
        /// </summary>
        /// <param name="context"> </param>
        public DvdStoreRepository(IDVDStoreDBContext context)
        {
            // Get the DbContext also check for null
            _dvdStoreDbContext = context ?? throw new ArgumentNullException(nameof(context));
            _dvdStorePropertyMapper = new DvdStorePropertyMapper();
        } // END of DvdStoreRepository Constructor

        //=====================================================================

        #endregion Constructor Methods

        #region Actor Methods

        /// <summary>
        /// ActorExists
        /// </summary>
        /// <param name="actorId"> </param>
        /// <returns> True or False (bool) </returns>
        public bool ActorExists(int actorId)
        {
            if (actorId != 0)
            {
                return _dvdStoreDbContext.Actors.Any(a => a.Actorid == actorId);
            }

            throw new ArgumentNullException(nameof(actorId));
        } // END of ActorExists

        //=====================================================================

        /// <summary>
        /// GetActorByFirstNameAndLastName
        /// </summary>
        /// <param name="firstname"> </param>
        /// <param name="lastname"> </param>
        /// <returns> </returns>
        public Actor GetActorByFirstNameAndLastName(string firstname, string lastname)
        {
            if (firstname == null)
            {
                throw new ArgumentNullException(nameof(firstname));
            }

            if (lastname == null)
            {
                throw new ArgumentNullException(nameof(lastname));
            }

            return _dvdStoreDbContext.Actors.FirstOrDefault(x => x.Firstname == firstname && x.Lastname == lastname);
        }

        /// <summary>
        /// GetActorById
        /// </summary>
        /// <param name="actorId"> </param>
        /// <returns> Actor object </returns>
        public Actor GetActorById(int actorId)
        {
            if (actorId != 0)
            {
                return _dvdStoreDbContext.Actors.FirstOrDefault(x => x.Actorid == actorId);
            }

            throw new ArgumentNullException(nameof(actorId));
        } // END of GetActorById

        //=====================================================================

        /// <summary>
        /// GetActorByIdWithFilmListing
        /// </summary>
        /// <param name="actorId"> </param>
        /// <returns>
        /// Actor object with nested collection of the FilmActor and Films tables
        /// </returns>
        public Actor GetActorByIdWithFilmListing(int actorId)
        {
            if (actorId != 0)
            {
                // LINQ to include the nested one to many collection. not the
                // bridge table called FilmActors which is the Actor and Film
                // tables have a one to many relation with (Many to Many)
                var result = _dvdStoreDbContext.Actors
                    .Include(actor => actor.Filmactors)
                    .ThenInclude(filmActor => filmActor.Film)
                    .ThenInclude(film => film.Filmactors)
                    .FirstOrDefault(x => x.Actorid == actorId);

                return result;
            }

            throw new ArgumentNullException(nameof(actorId));
        } // END of GetActorByIdWithFilmListing

        //=====================================================================

        // END of GetActorById
        //=====================================================================

        /// <summary>
        /// GetActorFilmListByFirstNameLastName
        /// </summary>
        /// <param name="firstName"> </param>
        /// <param name="lastname"> </param>
        /// <returns> IEnumerable List of ActorFilmListing class </returns>
        public IEnumerable<ActorFilmListing> GetActorFilmListByFirstNameLastName(string firstname, string lastname)
        {
            var actorFilmListing = (from a in _dvdStoreDbContext.Actors
                                    join fa in _dvdStoreDbContext.Filmactors on a.Actorid equals fa.Actorid
                                    join f in _dvdStoreDbContext.Films on fa.Filmid equals f.Filmid
                                    select new ActorFilmListing
                                    {
                                        ActorId = a.Actorid,
                                        FirstName = a.Firstname,
                                        LastName = a.Lastname,
                                        FilmId = f.Filmid,
                                        FilmTitle = f.Title,
                                        Rating = f.Rating
                                    })
                .Where(a => a.LastName == lastname && a.FirstName == firstname)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToList();

            return actorFilmListing;
        }

        /// <summary>
        /// GetActorFilmListById
        /// </summary>
        /// <param name="actorId"> </param>
        /// <returns> IEnumerable List of the ActorFilmListing class </returns>
        public IEnumerable<ActorFilmListing> GetActorFilmListById(int actorId)
        {
            var actorFilmListing = (from a in _dvdStoreDbContext.Actors
                                    join fa in _dvdStoreDbContext.Filmactors on a.Actorid equals fa.Actorid
                                    join f in _dvdStoreDbContext.Films on fa.Filmid equals f.Filmid
                                    select new ActorFilmListing
                                    {
                                        ActorId = a.Actorid,
                                        FirstName = a.Firstname,
                                        LastName = a.Lastname,
                                        FilmId = f.Filmid,
                                        FilmTitle = f.Title,
                                        Rating = f.Rating
                                    })
                .Where(a => a.ActorId == actorId)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToList();

            return actorFilmListing;
        }

        /// <summary> GetActors </summary> <returns> IEnumerable<Actor> List </returns>
        public IEnumerable<Actor> GetActors()
        {
            return _dvdStoreDbContext.Actors.ToList();
        } // END of GetActors

        //=====================================================================

        /// <summary>
        /// GetActors
        /// </summary>
        /// <param name="actorResourceParameters"> </param>
        /// <returns> PagedList of the Actor class </returns>
        /// <remarks>
        /// This uses a ResourceParameters class in conjunction with the
        /// PagedList Class to return paged data from the database
        /// </remarks>
        public PagedList<Actor> GetActors(ActorResourceParameters actorResourceParameters)
        {
            //=================================================================
            // Check parameters
            //=================================================================

            // Check for null
            if (actorResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(actorResourceParameters));
            }

            // Setup IQueryable for table object we are going to get data for.
            // We load the collection var accordingly as we process the resource
            // parameter object passed into the repository
            var collection = _dvdStoreDbContext.Actors as IQueryable<Actor>;

            // Check and run for the search parameter and get the collection for
            // columns we have chosen to allow search-able
            if (!string.IsNullOrWhiteSpace(actorResourceParameters.SearchQuery))
            {
                var searchQuery = actorResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Firstname.Contains(searchQuery)
                                                   || a.Lastname.Contains(searchQuery));
            }

            // Next check the orderby parameter and then apply the sort
            if (!string.IsNullOrWhiteSpace(actorResourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var actorPropertyMappingDictionary =
                    _dvdStorePropertyMapper.GetPropertyMapping<ActorDto, Actor>();

                collection = collection.ApplySort(actorResourceParameters.OrderBy,
                    actorPropertyMappingDictionary);
            }

            // FINALLY run the collection through the Paging process
            return PagedList<Actor>.Create(collection,
                actorResourceParameters.PageNumber,
                actorResourceParameters.PageSize);
        } // END of PagedList<Actor> GetActors

        //=====================================================================

        // END of GetActorFilmListById
        //=====================================================================

        // END of IEnumerable<ActorFilmListing>
        //=====================================================================

        /// <summary>
        /// GetActorWithFilmListReport
        /// </summary>
        /// <param name="actorResourceParameters"> </param>
        /// <returns> PagedList of the ActorFilmListing class </returns>
        public PagedList<ActorFilmListing> GetActorWithFilmListReport(ActorResourceParameters actorResourceParameters)
        {
            //=================================================================
            // Check parameters
            //=================================================================

            // Check for null
            if (actorResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(actorResourceParameters));
            }

            var collection = (from a in _dvdStoreDbContext.Actors
                              join fa in _dvdStoreDbContext.Filmactors on a.Actorid equals fa.Actorid
                              join f in _dvdStoreDbContext.Films on fa.Filmid equals f.Filmid
                              select new ActorFilmListing
                              {
                                  ActorId = a.Actorid,
                                  FirstName = a.Firstname,
                                  LastName = a.Lastname,
                                  FilmId = f.Filmid,
                                  FilmTitle = f.Title,
                                  Rating = f.Rating
                              })
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .AsQueryable();

            // Setup IQueryable for table object we are going to get data for.
            // We load the collection var accordingly as we process the
            // resource parameter object passed into the repository
            //var collection = _dvdStoreDbContext.Actor as IQueryable<ActorFilmListing>;

            // Check and run for the search parameter and get the collection for
            // columns we have chosen to allow search-able
            if (!string.IsNullOrWhiteSpace(actorResourceParameters.SearchQuery))
            {
                var searchQuery = actorResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.FirstName.Contains(searchQuery)
                                                   || a.LastName.Contains(searchQuery) ||
                                                   a.FilmTitle.Contains(searchQuery));
            }

            // Next check the orderby parameter and then apply the sort
            if (!string.IsNullOrWhiteSpace(actorResourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var actorPropertyMappingDictionary =
                    _dvdStorePropertyMapper.GetPropertyMapping<ActorFilmListing, ActorFilmListing>();

                collection = collection.ApplySort(actorResourceParameters.OrderBy,
                    actorPropertyMappingDictionary);
            }

            // FINALLY run the collection through the Paging process
            return PagedList<ActorFilmListing>.Create(collection,
                actorResourceParameters.PageNumber,
                actorResourceParameters.PageSize);
        } // END of GetActorWithFilmListReport\

        //=====================================================================

        #endregion Actor Methods

        #region Actor CRUD Methods

        public void AddActor(Actor actor)
        {
            if (actor == null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                ArgumentNullException argumentNullException = new ArgumentNullException("Actor argument is missing or null");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
                throw argumentNullException;
            }
            _dvdStoreDbContext.Actors.Add(actor);
        }

        /// <summary>
        /// DeleteActor
        /// </summary>
        /// <param name="actor"> </param>
        public void DeleteActor(Actor actor)
        {
            // TODO: Intentionally did not complete fix for deleting data due to FK, etc. - RG to spend time on more important code for HATEOAS.
            _dvdStoreDbContext.Actors.Remove(actor);
        }

        public void UpdateActor(Actor actor)
        {
            // No code is needed here as the context object is tracking changes
            // by auto-mapper. So once this method is called mapping of changes
            // been staged by EF and only the call later of Save on this
            // repository will persist the data to the database.
        }

        #endregion Actor CRUD Methods

        #region Save Methods

        /// <summary>
        /// Save
        /// </summary>
        /// <returns> bool </returns>
        public bool Save()
        {
            return _dvdStoreDbContext.SaveChanges() >= 0;
        }

        #endregion Save Methods

        #region Film Methods

        /// <summary>
        /// GetFilm
        /// </summary>
        /// <param name="filmId"> </param>
        /// <returns> Gets a single film object by Id </returns>
        public Film GetFilm(int filmId)
        {
            var actorFilm = _dvdStoreDbContext.Films.FirstOrDefault(x => x.Filmid == filmId);

            return actorFilm;
        }

        /// <summary>
        /// GetFilm
        /// </summary>
        /// <param name="actorId"> </param>
        /// <param name="filmId"> </param>
        /// <returns> Gets a single film object for specific actor </returns>
        public Film GetFilm(int actorId, int filmId)
        {
            var actorFilm = (from f in _dvdStoreDbContext.Films
                             join fa in _dvdStoreDbContext.Filmactors on f.Filmid equals fa.Filmid
                             where fa.Actorid == actorId
                             select new Film
                             {
                                 Title = f.Title,
                                 Filmid = f.Filmid,
                                 Description = f.Description,
                                 Releaseyear = f.Releaseyear,
                                 Languageid = f.Languageid,
                                 Originallanguageid = f.Languageid,
                                 Rentalduration = f.Rentalduration,
                                 Rentalrate = f.Rentalrate,
                                 Length = f.Length,
                                 Replacementcost = f.Replacementcost,
                                 Rating = f.Rating,
                                 Specialfeatures = f.Specialfeatures,
                                 Lastupdate = f.Lastupdate,
                                 Language = f.Language
                             }).FirstOrDefault(x => x.Filmid == filmId);

            return actorFilm;
        }

        /// <summary>
        /// GetFilms
        /// </summary>
        /// <returns> IEnumerable List of the Film class </returns>
        public IEnumerable<Film> GetFilms()
        {
            return _dvdStoreDbContext.Films.ToList();
        } // END of GetFilms

        //=====================================================================

        /// <summary>
        /// GetFilms
        /// </summary>
        /// <param name="actorId"> </param>
        /// <returns> IEnumerable List of the Film class </returns>
        public IEnumerable<Film> GetFilms(int actorId)
        {
            var actorFilmListing = from f in _dvdStoreDbContext.Films
                                   join fa in _dvdStoreDbContext.Filmactors on f.Filmid equals fa.Filmid
                                   where fa.Actorid == actorId
                                   select new Film
                                   {
                                       Title = f.Title,
                                       Filmid = f.Filmid,
                                       Description = f.Description,
                                       Releaseyear = f.Releaseyear,
                                       Languageid = f.Languageid,
                                       Originallanguageid = f.Languageid,
                                       Rentalduration = f.Rentalduration,
                                       Rentalrate = f.Rentalrate,
                                       Length = f.Length,
                                       Replacementcost = f.Replacementcost,
                                       Rating = f.Rating,
                                       Specialfeatures = f.Specialfeatures,
                                       Lastupdate = f.Lastupdate,
                                       Language = f.Language
                                   };

            return actorFilmListing;
        } // END of GetFilms(int actorId)

        //=====================================================================

        // END of GetFilm(int filmId)
        //=====================================================================

        // END of GetFilm(int actorId, int filmId)
        //=====================================================================

        /// <summary>
        /// GetFilms
        /// </summary>
        /// <param name="filmResourceParameters"> </param>
        /// <returns> PagedList of the Film class </returns>
        public PagedList<Film> GetFilms(FilmResourceParameters filmResourceParameters)
        {
            //=================================================================
            // Check parameters
            //=================================================================

            // Check for null
            if (filmResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(filmResourceParameters));
            }

            // Setup IQueryable for table object we are going to get data for.
            // We load the collection var accordingly as we process the resource
            // parameter object passed into the repository
            var collection = _dvdStoreDbContext.Films as IQueryable<Film>;

            // Check and run for the search parameter and get the collection for
            // columns we have chosen to allow search-able
            if (!string.IsNullOrWhiteSpace(filmResourceParameters.SearchQuery))
            {
                var searchQuery = filmResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Title.Contains(searchQuery)
                                                   || a.Description.Contains(searchQuery));
            }

            // Next check the orderby parameter and then apply the sort
            if (!string.IsNullOrWhiteSpace(filmResourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var actorPropertyMappingDictionary =
                    _dvdStorePropertyMapper.GetPropertyMapping<FilmDto, Film>();

                collection = collection.ApplySort(filmResourceParameters.OrderBy,
                    actorPropertyMappingDictionary);
            }

            // FINALLY run the collection through the Paging process
            return PagedList<Film>.Create(collection,
                filmResourceParameters.PageNumber,
                filmResourceParameters.PageSize);
        } // END of PagedList<Films> GetFilms

        //=====================================================================

        #endregion Film Methods

        #region Database Report Methods

        /// <summary>
        /// GetDatabaseStatistics
        /// </summary>
        /// <returns>
        /// IEnumerable List of the UspGetDatabaseStatisticsReturnModel class
        /// </returns>
        public IEnumerable<UspGetDatabaseStatisticsReturnModel> GetDatabaseStatistics()
        {
            var get5DatabaseStats = _dvdStoreDbContext.UspGetDatabaseStatistics();
            return get5DatabaseStats;
        } // END of GetDatabaseStatistics

        //=====================================================================

        #endregion Database Report Methods

        #region Clean Up

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

        #endregion Clean Up
    } // END of class DvdStoreRepository

    //=========================================================================
} // END of namespace DVDStore.DAL.Repositories

//=============================================================================

/*
 *
                 ____________________________________________________
  (>_____.----'||                                                    |
   /           ||                                                    |
  |---.   = /  ||                                                    |
  |    |  ( '  ||          Hard Working Repository Class             |
  |    |   `   ||                                                    |
  |---'        ||          We Haul Your Data Everywhere!!            |
  |            ||                                                    |
  [    ________||____________________________________________________|
  [__.'.---.   |[Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y__Y]
  [   //.-.\\__| `.__//.-.\\//.-.\\_________________//.-.\\//.-.\\_.'\
  [__/( ( ) )`      '( ( ) )( ( ) )`               '( ( ) )( ( ) )`
-------`---'----------`---'--`---'-------------------`---'--`---'------

 *
 */