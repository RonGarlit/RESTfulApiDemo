using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Customer
    {
        #region Public Constructors

        public Customer()
        {
            Payments = new HashSet<Payment>();
            Rentals = new HashSet<Rental>();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Active { get; set; }
        public virtual Address Address { get; set; }
        public int Addressid { get; set; }
        public DateTime Createdate { get; set; }
        public int Customerid { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Lastupdate { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual Store Store { get; set; }
        public int Storeid { get; set; }

        #endregion Public Properties
    }
}