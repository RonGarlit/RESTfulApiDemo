using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Address
    {
        #region Public Constructors

        public Address()
        {
            Customers = new HashSet<Customer>();
            Stores = new HashSet<Store>();
            staff = new HashSet<staff>();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int Addressid { get; set; }
        public virtual City City { get; set; }
        public int Cityid { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public string District { get; set; }
        public DateTime Lastupdate { get; set; }
        public string Phone { get; set; }
        public string Postalcode { get; set; }
        public virtual ICollection<staff> staff { get; set; }
        public virtual ICollection<Store> Stores { get; set; }

        #endregion Public Properties
    }
}