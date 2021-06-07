using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Rental
    {
        #region Public Constructors

        public Rental()
        {
            Payments = new HashSet<Payment>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual Customer Customer { get; set; }
        public int Customerid { get; set; }
        public virtual Inventory Inventory { get; set; }
        public int Inventoryid { get; set; }
        public DateTime Lastupdate { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public DateTime Rentaldate { get; set; }
        public int Rentalid { get; set; }
        public DateTime? Returndate { get; set; }
        public virtual staff Staff { get; set; }
        public byte Staffid { get; set; }

        #endregion Public Properties
    }
}