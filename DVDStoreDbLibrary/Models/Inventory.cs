using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Inventory
    {
        #region Public Constructors

        public Inventory()
        {
            Rentals = new HashSet<Rental>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual Film Film { get; set; }
        public int Filmid { get; set; }
        public int Inventoryid { get; set; }
        public DateTime Lastupdate { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual Store Store { get; set; }
        public int Storeid { get; set; }

        #endregion Public Properties
    }
}