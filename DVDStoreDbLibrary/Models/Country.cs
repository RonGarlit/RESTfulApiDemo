using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Country
    {
        #region Public Constructors

        public Country()
        {
            Cities = new HashSet<City>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual ICollection<City> Cities { get; set; }
        public string Country1 { get; set; }
        public short Countryid { get; set; }
        public DateTime? Lastupdate { get; set; }

        #endregion Public Properties
    }
}