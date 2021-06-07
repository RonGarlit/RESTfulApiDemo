#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class TempActorFilmListing
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