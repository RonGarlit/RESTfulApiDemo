using System.Collections.Generic;

namespace Api.Helpers.PropMapHelpers
{
    public interface IPropertyMapper
    {
        #region Public Methods

        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        bool ValidMappingExistsFor<TSource, TDestination>(string fields);

        #endregion Public Methods
    }
}