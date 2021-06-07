using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Actor
    {
        #region Public Constructors

        public Actor()
        {
            Filmactors = new HashSet<Filmactor>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int Actorid { get; set; }
        public virtual ICollection<Filmactor> Filmactors { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}