namespace DVDStore.Common.ResourceParameters.v1_0
{
    public class ActorResourceParameters
    {
        #region Private Fields

        private const int maxPageSize = 20;

        private int _pageSize = 10;

        #endregion Private Fields

        #region Public Properties

        public string Fields { get; set; }
        public string OrderBy { get; set; } = "Actorid";
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }

        public string SearchQuery { get; set; }

        #endregion Public Properties

        // Setup default value here based on model class properties
    }
}