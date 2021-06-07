using System;

namespace DVDStore.Common.Models.v1_1.Dto
{
    public class ActorForUpdateDto
    {
        #region Public Properties

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Lastupdate { get; set; }

        #endregion Public Properties
    }
}