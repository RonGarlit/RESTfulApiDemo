// /********************************************************************************** **
// **  RESTfulApiPrototype v1.1 **
// **  Copyright 2020
// **  Developed by: Ronald Garlit. **
// **  This software is the proprietary information of Ronald Garlit. **
// **  Use is subject to license terms.
// *********************************************************************************** **
// **  FileName: IDvdStoreRepository.cs (DVDStore.DAL.Repositories)
// **  Version: 0.1
// **  Author: Ronald Garlit **
// **  Description: **
// **  Interface class for the DVDStore repository class. **
// **  Change History **
// **  WHEN WHO WHAT 
// **---------------------------------------------------------------------------------
// **  2020-11-21 rgarlit STARTED DEVELOPMENT
// **  2021-01-18 rgarlit Working on RESTful API Level 4 Versioning in place
// **  2021-02-15 rgarlit Working on RESTful API Level 3 stuff
// **  2021-02-18 rgarlit Working on Data Shaping stuff
// ***********************************************************************************/

using Api.Helpers;
using DVDStore.Common.Models.v1_1;
using DVDStore.Common.ResourceParameters.v1_1;
using DVDStore.DAL.Models;
using System.Collections.Generic;

namespace DVDStore.DAL.Repositories.v1_1
{
    /// <summary>
    ///     IDvdStoreRepository
    /// </summary>
    public interface IDvdStoreRepository
    {
        #region Public Methods

        bool ActorExists(int actorId);

        void AddActor(Actor actor);

        void DeleteActor(Actor actor);

        Actor GetActorByFirstNameAndLastName(string firstname, string lastname);

        Actor GetActorById(int actorId);

        Actor GetActorByIdWithFilmListing(int actorId);

        IEnumerable<ActorFilmListing> GetActorFilmListByFirstNameLastName(string firstName, string lastname);

        IEnumerable<ActorFilmListing> GetActorFilmListById(int actorId);

        IEnumerable<Actor> GetActors();

        PagedList<Actor> GetActors(ActorResourceParameters actorResourceParameters);

        PagedList<ActorFilmListing> GetActorWithFilmListReport(ActorResourceParameters actorResourceParameters);

        IEnumerable<UspGetDatabaseStatisticsReturnModel> GetDatabaseStatistics();

        Film GetFilm(int actorId, int filmId);

        Film GetFilm(int filmId);

        IEnumerable<Film> GetFilms();

        PagedList<Film> GetFilms(FilmResourceParameters filmResourceParameters);

        IEnumerable<Film> GetFilms(int actorId);

        bool Save();

        void UpdateActor(Actor actor);

        #endregion Public Methods
    } // END of interface IDvdStoreRepository

    //=========================================================================
} // END of namespace DVDStore.DAL.Repositories

//=============================================================================

/*
 *
Dilbert by Joan Stark

    (`'`'`'`')
     |      |
     |      |
    (|-()()-|)
     | (__) |
     |      |
     |______|
    /._/\/\_.\
   /  , /\    \
  ; / \\|| __\ ;
  |-|  './ \/|-|
  \ |   |    | /
   '\___|____/`
jgs  |--LI--|
     |  |   |
     |  |   |
     |  |   |
     |  |   |
     |  |   |
     |__|___|
 .----'=||='----.
 `""""`"  "`""""`

 *
 */