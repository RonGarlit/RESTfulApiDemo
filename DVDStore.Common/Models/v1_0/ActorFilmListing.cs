// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: ActorFilmListing.cs (DVDStore.Common)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  Model for us with Actor Film Listings in Repository.
// **
// **  Change History
// **
// **  WHEN        WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-11-06  rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

namespace DVDStore.Common.Models.v1_0
{
    public class ActorFilmListing
    {
        #region Public Properties

        public int ActorId { get; set; }
        public int FilmId { get; set; }
        public string FilmTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Rating { get; set; }

        #endregion Public Properties
    }
}