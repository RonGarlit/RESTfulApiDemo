#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Salesbystore
    {
        #region Public Properties

        public string Manager { get; set; }
        public string Store { get; set; }
        public int Storeid { get; set; }
        public decimal? Totalsales { get; set; }

        #endregion Public Properties
    }
}