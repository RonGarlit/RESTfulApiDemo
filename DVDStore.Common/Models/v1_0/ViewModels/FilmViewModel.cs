using System;

namespace DVDStore.Common.Models.v1_0.ViewModels
{
    public class FilmViewModel
    {
        #region Public Properties

        public string Description { get; set; }
        public int Filmid { get; set; }
        public byte Languageid { get; set; }
        public DateTime Lastupdate { get; set; }
        public short? Length { get; set; }
        public byte? Originallanguageid { get; set; }
        public string Rating { get; set; }
        public string Releaseyear { get; set; }
        public byte Rentalduration { get; set; }
        public decimal Rentalrate { get; set; }
        public decimal Replacementcost { get; set; }
        public string Specialfeatures { get; set; }
        public string Title { get; set; }

        #endregion Public Properties
    }
}