using System;
using System.Collections.Generic;

namespace DVDStore.Common.Models.v1_0.Dto
{
    public class ActorDto
    {
        #region Public Properties

        public int Actorid { get; set; }

        public List<FilmactorDto> Filmactors { get; set; }
            = new List<FilmactorDto>();

        public string Firstname { get; set; }
        public string FullName { get; set; }
        public string Lastname { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}