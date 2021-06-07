using System;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Filmactor
    {
        #region Public Properties

        public virtual Actor Actor { get; set; }
        public int Actorid { get; set; }
        public virtual Film Film { get; set; }
        public int Filmid { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}