using System;
using System.Collections.Generic;

#nullable disable

namespace DVDStore.DAL.Models
{
    public partial class Category
    {
        #region Public Constructors

        public Category()
        {
            Filmcategories = new HashSet<Filmcategory>();
        }

        #endregion Public Constructors

        #region Public Properties

        public byte Categoryid { get; set; }
        public virtual ICollection<Filmcategory> Filmcategories { get; set; }
        public DateTime Lastupdate { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }
}