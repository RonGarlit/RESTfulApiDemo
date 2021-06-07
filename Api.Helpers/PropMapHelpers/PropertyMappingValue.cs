using System;
using System.Collections.Generic;

namespace Api.Helpers.PropMapHelpers
{
    public class PropertyMappingValue
    {
        #region Public Constructors

        public PropertyMappingValue(IEnumerable<string> destinationProperties,
            bool revert = false)
        {
            DestinationProperties = destinationProperties
                ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; private set; }

        #endregion Public Properties
    }
}