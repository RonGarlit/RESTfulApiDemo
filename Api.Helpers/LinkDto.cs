namespace Api.Helpers
{
    public class LinkDto
    {
        #region Public Constructors

        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Href { get; private set; }
        public string Method { get; private set; }
        public string Rel { get; private set; }

        #endregion Public Properties
    }
}