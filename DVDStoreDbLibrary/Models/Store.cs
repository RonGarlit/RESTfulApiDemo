using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Store
    {
        #region Public Constructors

        public Store()
        {
            Customers = new HashSet<Customer>();
            Inventories = new HashSet<Inventory>();
            staff = new HashSet<staff>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual Address Address { get; set; }
        public int Addressid { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public DateTime Lastupdate { get; set; }
        public virtual staff Managerstaff { get; set; }
        public byte Managerstaffid { get; set; }
        public virtual ICollection<staff> staff { get; set; }
        public int Storeid { get; set; }

        #endregion Public Properties
    }
}