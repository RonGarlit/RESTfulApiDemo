using System;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Payment
    {
        #region Public Properties

        public decimal Amount { get; set; }
        public virtual Customer Customer { get; set; }
        public int Customerid { get; set; }
        public DateTime Lastupdate { get; set; }
        public DateTime Paymentdate { get; set; }
        public int Paymentid { get; set; }
        public virtual Rental Rental { get; set; }
        public int? Rentalid { get; set; }
        public virtual staff Staff { get; set; }
        public byte Staffid { get; set; }

        #endregion Public Properties
    }
}