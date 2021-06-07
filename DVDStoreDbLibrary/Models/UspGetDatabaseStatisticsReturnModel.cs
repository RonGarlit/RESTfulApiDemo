namespace DVDStore.DAL.Models
{
    public partial class UspGetDatabaseStatisticsReturnModel
    {
        #region Public Properties

        public string DataSpaceUsed { get; set; }
        public string IndexSpaceUsed { get; set; }
        public int? Rows { get; set; }
        public string SpaceReservedUsed { get; set; }
        public string TableName { get; set; }
        public string UnusedSpace { get; set; }

        #endregion Public Properties
    }
}