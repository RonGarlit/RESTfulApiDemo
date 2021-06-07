using System;
using System.Collections.Generic;

namespace DVDStore.Common.Models.v1_0.ViewModels
{
    public class ActorFilmListViewModel
    {
        #region Public Properties

        public List<FilmViewModel> ActorFilmList { get; set; }
        public int Actorid { get; set; }
        public string Firstname { get; set; }
        public string FullName { get; set; }
        public string Lastname { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}