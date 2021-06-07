using System;
using System.Collections.Generic;

namespace Api.Helpers.PropMapHelpers
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        #region Public Constructors

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            MappingDictionary = mappingDictionary ??
                throw new ArgumentNullException(nameof(mappingDictionary));
        }

        #endregion Public Constructors

        #region Public Properties

        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

        #endregion Public Properties
    }
}