using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Film
    {
        #region Public Constructors

        public Film()
        {
            Filmactors = new HashSet<Filmactor>();
            Filmcategories = new HashSet<Filmcategory>();
            Inventories = new HashSet<Inventory>();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Description { get; set; }
        public virtual ICollection<Filmactor> Filmactors { get; set; }
        public virtual ICollection<Filmcategory> Filmcategories { get; set; }
        public int Filmid { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual Language Language { get; set; }
        public byte Languageid { get; set; }
        public DateTime Lastupdate { get; set; }
        public short? Length { get; set; }
        public virtual Language Originallanguage { get; set; }
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