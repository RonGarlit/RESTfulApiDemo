using Api.Helpers.PropMapHelpers;
using DVDStore.Common.Models.v1_1;
using DVDStore.Common.Models.v1_1.Dto;
using DVDStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DVDStore.Common.PropertyMapping.v1_1
{
    public class DvdStorePropertyMapper : IDvdStorePropertyMapper
    {
        #region Private Fields

        //====================================================================
        // TODO: Setup property mapping with dictionary for each object needing mapping for the ORDER BY feature
        // Actor Database Model Mapping
        //====================================================================
        private readonly Dictionary<string, PropertyMappingValue> _actorPropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
              // TODO: Add appropriate items as needed for property mapping
              // Example of Mapping
              //{ "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
              //{ "MainCategory", new PropertyMappingValue(new List<string>() { "MainCategory" } )},
              //{ "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },
              //{ "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
              { "Actorid", new PropertyMappingValue(new List<string>() { "Actorid" } ) },
              { "Firstname", new PropertyMappingValue(new List<string>() { "Firstname" } )},
              { "Lastname", new PropertyMappingValue(new List<string>() { "Lastname" } )}
              //{ "Name", new PropertyMappingValue(new List<string>() { "Firstname", "Lastname" }) }
          };
        //====================================================================
        // TODO: Setup property mapping with dictionary for each object needing mapping for the ORDER BY feature
        // Film Database Model Mapping
        //====================================================================
        private readonly Dictionary<string, PropertyMappingValue> _filmPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
		        // TODO: Add appropriate items as needed for property mapping
		        // Example of Mapping
		        //{ "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
		        //{ "MainCategory", new PropertyMappingValue(new List<string>() { "MainCategory" } )},
		        //{ "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },
		        //{ "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
		        { "Filmid", new PropertyMappingValue(new List<string>() { "Filmid" } )},
                { "Title", new PropertyMappingValue(new List<string>() { "Title" } )},
                { "Description", new PropertyMappingValue(new List<string>() { "Description" } )}
            };

        //====================================================================
        // TODO: Setup property mapping with dictionary for each object needing mapping for the ORDER BY feature
        //TempActorFilmListing Database Model Mapping
        //====================================================================
        private readonly Dictionary<string, PropertyMappingValue> _actorFilmListingPropertyMapping =
                    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                    {
                // TODO: Add appropriate items as needed for property mapping
                // Example of Mapping
                { "ActorId", new PropertyMappingValue(new List<string>() { "ActorId" } ) },
                { "FirstName", new PropertyMappingValue(new List<string>() { "FirstName" } )},
                { "LastName", new PropertyMappingValue(new List<string>() { "LastName" } )},
                { "FilmId", new PropertyMappingValue(new List<string>() { "FilmId" } ) },
                { "FilmTitle", new PropertyMappingValue(new List<string>() { "FilmTitle" } )},
                { "Rating", new PropertyMappingValue(new List<string>() { "Rating" } )}
                    };

        // variable to hold the property mappings list
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     DvdStorePropertyMapper Constructor
        /// </summary>
        public DvdStorePropertyMapper()
        {
            // TODO: Add appropriate items as needed for property mapping
            // Example of mapping
            //_propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_exampleAuthorPropertyMapping));

            // Add property mapping to the property mapping list to be
            // used by the DvdStoreRepository in PagedLists of objects
            _propertyMappings.Add(new PropertyMapping<ActorDto, Actor>(_actorPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<FilmDto, Film>(_filmPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<ActorFilmListing, ActorFilmListing>(_actorFilmListingPropertyMapping));
        }

        #endregion Public Constructors

        // END of DvdStorePropertyMapper Constructor

        //=====================================================================

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