// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: Program.cs (DVDStoreTestConsole)
// **  Version: 0.1
// **  Author: Ronald A. Garlit 
// **
// **  Description: 
// **
// **  Console app to test the manual addition of Stored Procedures and
// **  the DVDStoreDBContextRevised.
// **
// **  Change History
// **
// **  WHEN        WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-10-27  rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

using System;
using System.IO;
using System.Linq;
using DVDStore.Common.Models.v1_0;
using DVDStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DVDStore.Test.ConsoleApp
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            //=================================================================
            // Setup the configuration so you can get items in the AppSettings.json file
            //=================================================================
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            //=================================================================
            // Get a connectionString from the AppSettings.json file
            //=================================================================
            var connectionString = configuration.GetConnectionString("DVDStoreDb");

            //=================================================================
            // THIS IS HOW YOU BUILD THE 'DbContextOptions'
            //=================================================================
            var options = new DbContextOptionsBuilder<DVDStoreDBContext>()
                .UseSqlServer(connectionString)
                .Options;
            //=================================================================

            // Regular DbContext - Normal Dotnet Created DbContext
            var dvdDb = new DVDStoreDBContext(options);

            // Revised DbContext to accept a ConnectionString in
            // an additional constructor
            var dvdDbRevised = new DVDStoreDBContextRevised(configuration.GetConnectionString("DVDStoreDb"));

            // Set console colors through out using these commands
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine(
                "Test of EF Core/.NET 5.0 - Used to generate code from the DVD Store (Sakila) Sample Database...");
            Console.WriteLine(
                "========================================================================================================\n\n");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "========================================================================================================");
            Console.WriteLine(
                "NOTE: I had to hand code for stored procedures as that hasn't been built onto dotnet ef command.");
            Console.WriteLine(
                "      This requires hand creation of a model and adding it as entity in the OnModelCreating of DbContext");
            Console.WriteLine("      using Fluent method with the ModelBuilder.");
            Console.WriteLine(
                "========================================================================================================\n\n");


            TestUsingDvdDbDbContext(dvdDb);

            TestUsingDvdDbRevisedDbContext(dvdDbRevised);

           
            // Restore the original console colors.
            Console.ResetColor();
        }

        private static void TestUsingDvdDbDbContext(DVDStoreDBContext dvdDb)
        {
            using (dvdDb)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Normal DbContext LINQ call for data from a table count");
                var actorRecordCount = dvdDb.Actors.ToList().Count;
                Console.WriteLine($"Actor Table Record Count:  {actorRecordCount}");
                Console.WriteLine(
                    "========================================================================================================");


                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatistic() \n");

                var getDvdStoreDatabaseStatistics = dvdDb.UspGetDatabaseStatistics();

                foreach (var storeDbStat in getDvdStoreDatabaseStatistics)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatisticsAsync() \n");
                var getDvdStoreDatabaseStatisticsAsync = dvdDb.UspGetDatabaseStatisticsAsync();

                foreach (var storeDbStat in getDvdStoreDatabaseStatisticsAsync.Result)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategory() \n");

                var getDvdStorepGetListOfTitlesByCategory = dvdDb.UspGetListOfTitlesByCategory("Sci-Fi");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategory)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategoryAsync() \n");
                var getDvdStorepGetListOfTitlesByCategoryAsync = dvdDb.UspGetListOfTitlesByCategoryAsync("Documentary");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategoryAsync.Result)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                var actorFilmListing = (from a in dvdDb.Actors
                        join fa in dvdDb.Filmactors on a.Actorid equals fa.Actorid
                        join f in dvdDb.Films on fa.Filmid equals f.Filmid
                        select new ActorFilmListing()
                        {
                            ActorId = a.Actorid,
                            FirstName = a.Firstname,
                            LastName = a.Lastname,
                            FilmId = f.Filmid,
                            FilmTitle = f.Title,
                            Rating = f.Rating
                        })
                    .Where(a => a.LastName == "CHASE" && a.FirstName == "ED")
                    .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .ToList();

                var generalCount = actorFilmListing.Count;

                Console.WriteLine($"Films made by: {actorFilmListing[0].FirstName} {actorFilmListing[0].LastName}");

                foreach (var actor in actorFilmListing)
                {
                    Console.WriteLine($"{actor.FilmTitle}");
                }

                Console.WriteLine(
                    "========================================================================================================");
            } // END of using (dvdDb)
        }

        private static void TestUsingDvdDbRevisedDbContext(DVDStoreDBContextRevised dvdDbRevised)
        {
            using (dvdDbRevised)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    "========================================================================================================");
                Console.WriteLine("Normal DbContext LINQ call for data from a table count");
                var paymentRecordCount = dvdDbRevised.Payments.ToList().Count;
                Console.WriteLine($"Payment Table Record Count:  {paymentRecordCount}");
                Console.WriteLine(
                    "========================================================================================================");


                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatistic() \n");

                var getDvdStoreDatabaseStatistics = dvdDbRevised.UspGetDatabaseStatistics();

                foreach (var storeDbStat in getDvdStoreDatabaseStatistics)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n Calling Stored Procedure UspGetDatabaseStatisticsAsync() \n");
                var getDvdStoreDatabaseStatisticsAsync = dvdDbRevised.UspGetDatabaseStatisticsAsync();

                foreach (var storeDbStat in getDvdStoreDatabaseStatisticsAsync.Result)
                {
                    Console.WriteLine($"Table Name: {storeDbStat.TableName} has {storeDbStat.Rows} Rows.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategory() \n");

                var getDvdStorepGetListOfTitlesByCategory = dvdDbRevised.UspGetListOfTitlesByCategory("Classics");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategory)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n Calling Stored Procedure UspGetListOfTitlesByCategoryAsync() \n");
                var getDvdStorepGetListOfTitlesByCategoryAsync =
                    dvdDbRevised.UspGetListOfTitlesByCategoryAsync("Travel");

                foreach (var storeDbTitleListByCat in getDvdStorepGetListOfTitlesByCategoryAsync.Result)
                {
                    Console.WriteLine(
                        $"Movie Title: {storeDbTitleListByCat.title} in the {storeDbTitleListByCat.name} Category.");
                }

                Console.WriteLine(
                    "========================================================================================================");
            } // END of using (dvdDbRevised)
        }
        // END of static void Main(string[] args)
        //=====================================================================

    } // END of class Program
    //=========================================================================

} // END of namespace DVDStoreTestConsole 
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