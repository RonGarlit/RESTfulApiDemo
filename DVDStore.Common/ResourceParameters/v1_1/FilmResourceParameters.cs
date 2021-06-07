namespace DVDStore.Common.ResourceParameters.v1_1
{
    public class FilmResourceParameters
    {
        #region Private Fields

        private const int maxPageSize = 20;

        private int _pageSize = 10;

        #endregion Private Fields

        #region Public Properties

        public string Fields { get; set; }
        public string OrderBy { get; set; } = "Filmid";
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