namespace Api.Helpers.PropMapHelpers
{
    public interface IPropertyCheckerService
    {
        #region Public Methods

        bool TypeHasProperties<T>(string fields);

        #endregion Public Methods
    }
}