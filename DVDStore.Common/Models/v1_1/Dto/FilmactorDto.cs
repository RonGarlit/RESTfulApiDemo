using System;

namespace DVDStore.Common.Models.v1_1.Dto
{
    public class FilmactorDto
    {
        #region Public Properties

        public ActorDto Actor { get; set; }
        public int Actorid { get; set; }
        public FilmDto Film { get; set; }
        public int Filmid { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}