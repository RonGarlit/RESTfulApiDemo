using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class staff
    {
        #region Public Constructors

        public staff()
        {
            Payments = new HashSet<Payment>();
            Rentals = new HashSet<Rental>();
        }

        #endregion Public Constructors

        #region Public Properties

        public bool? Active { get; set; }
        public virtual Address Address { get; set; }
        public int Addressid { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Lastupdate { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public byte[] Picture { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public byte Staffid { get; set; }
        public virtual Store Store { get; set; }
        public int Storeid { get; set; }
        public virtual Store StoreNavigation { get; set; }
        public string Username { get; set; }

        #endregion Public Properties
    }
}