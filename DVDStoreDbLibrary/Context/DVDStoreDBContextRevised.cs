// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: DVDStoreDBContextRevised.cs (DVDStoreDbLibrary)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  This was a test DbContext that inherited the original
// **  DVDStoreDBContext. This one added a constructor to allow the the
// **  ConnectionString to be passed in.
// **
// **  Change History
// **
// **  WHEN         WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-10-27   rgarlit     STARTED
// **  2020-11-16   rgarlit     Revised for EF Core 5.0 - with Many to many changes
// ***********************************************************************************/

using Microsoft.EntityFrameworkCore;

namespace DVDStore.DAL.Context
{
    public partial class DVDStoreDBContextRevised : DVDStoreDBContext
    {
        #region Private Fields

        // Internal ConnectionString variable
        private readonly string _connectionString;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     DVDStoreDBContextRevised
        /// </summary>
        /// <param name="connectionString"></param>
        /// <remarks>
        ///     Additional constructor that takes a ConnectionString parameters.
        /// </remarks>
        public DVDStoreDBContextRevised(string connectionString)
        {
            this._connectionString = connectionString;
        }

        #endregion Public Constructors

        // END of DVDStoreDBContextRevised(string connectionString) Constructor

        //=====================================================================

        #region Protected Methods

        /// <summary>
        ///     OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <remarks>
        ///     This is the overriden OnConfiguring method that uses the connection string being passed in.
        /// </remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        #endregion Protected Methods

        // END of override of OnConfiguring

        //=====================================================================
    } // END of  partial class DVDStoreDBContextRevised : DVDStoreDBContext

    //=========================================================================
} // END of namespace DVDStoreDbLibrary.Context

//=============================================================================

/*
 *
                       .-.
        .-""`""-.    |(@ @)
     _/`oOoOoOoOo`\_ \ \-/
    '.-=-=-=-=-=-=-.' \/ \
rag   `-=.=-.-=.=-'    \ /\
         ^  ^  ^       _H_ \

 *
 *
 */