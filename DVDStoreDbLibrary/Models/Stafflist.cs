﻿#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Stafflist
    {
        #region Public Properties

        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Sid { get; set; }
        public string Zipcode { get; set; }

        #endregion Public Properties
    }
}