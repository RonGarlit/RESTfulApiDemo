#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Filmlist
    {
        #region Public Properties

        public string Actors { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int? Fid { get; set; }
        public short? Length { get; set; }
        public decimal? Price { get; set; }
        public string Rating { get; set; }
        public string Title { get; set; }

        #endregion Public Properties
    }
}