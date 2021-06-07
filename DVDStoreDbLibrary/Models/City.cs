using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class City
    {
        #region Public Constructors

        public City()
        {
            Addresses = new HashSet<Address>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual ICollection<Address> Addresses { get; set; }
        public string City1 { get; set; }
        public int Cityid { get; set; }
        public virtual Country Country { get; set; }
        public short Countryid { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}