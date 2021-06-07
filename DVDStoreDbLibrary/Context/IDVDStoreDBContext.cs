// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: IDVDStoreDBContext.cs (DVDStore.DAL)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description: Program Description
// **
// **  <Enter Description Here!!>.
// **
// **  Change History
// **
// **  WHEN         WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-10-30   rgarlit     STARTED DEVELOPMENT
// **  2020-11-16   rgarlit     Revised for EF Core 5.0 - with Many to many changes
// ***********************************************************************************/

using DVDStore.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DVDStore.DAL.Context
{
    public interface IDVDStoreDBContext
    {
        #region Public Properties

        DbSet<Actor> Actors { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<Customerlist> Customerlists { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Filmactor> Filmactors { get; set; }
        DbSet<Filmcategory> Filmcategories { get; set; }
        DbSet<Filmlist> Filmlists { get; set; }
        DbSet<FilmRev> FilmRevs { get; set; }
        DbSet<Film> Films { get; set; }
        DbSet<Filmtext> Filmtexts { get; set; }
        DbSet<Inventory> Inventories { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<Rental> Rentals { get; set; }
        DbSet<Salesbyfilmcategory> Salesbyfilmcategories { get; set; }
        DbSet<Salesbystore> Salesbystores { get; set; }
        DbSet<staff> staff { get; set; }
        DbSet<Stafflist> Stafflists { get; set; }
        DbSet<Store> Stores { get; set; }
        DbSet<TempActorFilmListing> TempActorFilmListings { get; set; }

        #endregion Public Properties

        #region Public Methods

        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        string ToString();

        List<UspGetDatabaseStatisticsReturnModel> UspGetDatabaseStatistics();

        List<UspGetDatabaseStatisticsReturnModel> UspGetDatabaseStatistics(out int procResult);

        Task<List<UspGetDatabaseStatisticsReturnModel>> UspGetDatabaseStatisticsAsync();

        List<UspGetListOfTitlesByCategoryReturnModel> UspGetListOfTitlesByCategory(string categoryName);

        List<UspGetListOfTitlesByCategoryReturnModel> UspGetListOfTitlesByCategory(string categoryName,
            out int procResult);

        Task<List<UspGetListOfTitlesByCategoryReturnModel>> UspGetListOfTitlesByCategoryAsync(
            string categoryName);

        #endregion Public Methods
    }
}