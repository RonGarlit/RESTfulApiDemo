#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class FilmRev
    {
        #region Public Properties

        public byte Categoryid { get; set; }
        public int Filmid { get; set; }
        public short? Length { get; set; }
        public string Name { get; set; }
        public byte? Originallanguageid { get; set; }
        public string Rating { get; set; }
        public byte Rentalduration { get; set; }
        public decimal Rentalrate { get; set; }
        public decimal Replacementcost { get; set; }
        public string Specialfeatures { get; set; }
        public string Title { get; set; }

        #endregion Public Properties
    }
}