// Reference entities and models
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helpers.PropMapHelpers
{
    public class PropertyMapper : IPropertyMapper
    {
        #region Private Fields

        // TODO:
        private Dictionary<string, PropertyMappingValue> _exampleAuthorPropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
              // TODO: Add appropriate items as needed for property mapping
              // Example of Mapping
              //{ "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
              //{ "MainCategory", new PropertyMappingValue(new List<string>() { "MainCategory" } )},
              //{ "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },
              //{ "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
          };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        #endregion Private Fields

        #region Public Constructors

        public PropertyMapper()
        {
            // TODO: Add appropriate items as needed for property mapping
            // Example of mapping
            //_propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_exampleAuthorPropertyMapping));
        }

        #endregion Public Constructors

        #region Public Methods

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
           <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields
                // are coming from an orderBy string, this part must be
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Public Methods
    }
}