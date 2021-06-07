using System;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Filmcategory
    {
        #region Public Properties

        public virtual Category Category { get; set; }
        public byte Categoryid { get; set; }
        public virtual Film Film { get; set; }
        public int Filmid { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}