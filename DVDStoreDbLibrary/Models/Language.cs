using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Language
    {
        #region Public Constructors

        public Language()
        {
            FilmLanguages = new HashSet<Film>();
            FilmOriginallanguages = new HashSet<Film>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual ICollection<Film> FilmLanguages { get; set; }
        public virtual ICollection<Film> FilmOriginallanguages { get; set; }
        public byte Languageid { get; set; }
        public DateTime Lastupdate { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }
}