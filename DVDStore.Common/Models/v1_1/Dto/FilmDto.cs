using System;
using System.Collections.Generic;

namespace DVDStore.Common.Models.v1_1.Dto
{
    public class FilmDto
    {
        #region Public Properties

        public string Description { get; set; }
        public List<FilmactorDto> Filmactors { get; set; }
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