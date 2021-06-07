using Api.Helpers.PropMapHelpers;
using System.Collections.Generic;

namespace DVDStore.Common.PropertyMapping.v1_1
{
    public interface IDvdStorePropertyMapper
    {
        #region Public Methods

        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        bool ValidMappingExistsFor<TSource, TDestination>(string fields);

        #endregion Public Methods
    }
}