// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: MsTestDbUnitTestsV10.cs (DVDStore.Test.Repositories.UnitTests)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  Unit Testing for the repositories class against a real database.
// **  I may switch this to try out the EF In Memory Provider later
// **
// **  NO MOCKING YET.  Use this to test database named DvdStore during development.
// **  instead of a console application.  Yea I know this is not the TDD way.
// **  You want that look at the other test project DVDStore.Test.Repositories.MockedUnitTests
// **  There you will find using mocking with MOQ Framework.  :-)
// **
// **  Change History
// **
// **  WHEN         WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-11-12   rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

using DVDStore.Common.ResourceParameters.v1_0;
using DVDStore.DAL.Context;
using DVDStore.DAL.Repositories.v1_0;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DVDStore.Test.Repositories.DbUnitTests
{
    [TestClass]
    public class MsTestDbUnitTestsV10
    {
        #region Private Fields

        private IDVDStoreDBContext _dvdStoreDbContext;
        private IDvdStoreRepository _dvdStoreRepository;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void TestActors001_GetActorListCount()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var expectedValue = 200;

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testActors = _dvdStoreRepository.GetActors().ToList();
            var actorCount = testActors.Count;
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedValue, actorCount, "Expected Actor Count Did NOT Match!");
        }

        [TestMethod]
        public void TestActors002_GetActorById()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var testForActorid = 71;
            var expectedActorExistsValues = true;

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testDoesActorExist = _dvdStoreRepository.ActorExists(testForActorid);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedActorExistsValues, testDoesActorExist, $"Actor Exist test Failed!");
        }

        [TestMethod]
        public void TestActors003_GetActorByFirstNameAndLastName()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var testForActorid = 71;

            var testForAdam = "ADAM";
            var testForGrant = "GRANT";

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testActorResult = _dvdStoreRepository.GetActorByFirstNameAndLastName(testForAdam, testForGrant);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(testForActorid, testActorResult.Actorid, $"Actor Id Test Failed!");
            Assert.AreEqual(testForAdam, testActorResult.Firstname, $"Actor FirstName Test Failed!");
            Assert.AreEqual(testForGrant, testActorResult.Lastname, $"Actor Lastname Test Failed!");
        }

        [TestMethod]
        public void TestActors003_GetActorFilmListByFirstNameLastName()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************

            var testForAdam = "ADAM";
            var testForGrant = "GRANT";
            var testForAnnieIdentity = "ANNIE IDENTITY";

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testActorResult = _dvdStoreRepository.GetActorFilmListByFirstNameLastName(testForAdam, testForGrant);
            var testFindMovieTitle = testActorResult.FirstOrDefault(x => x.FilmTitle == testForAnnieIdentity).FilmTitle;
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(testForAnnieIdentity, testFindMovieTitle, $"Movie title did not match {testForAnnieIdentity}");
        }

        [TestMethod]
        public void TestActors004_GetActorsIEnumerableList()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var expectedRecordActorId = 10;
            var expectedRecordFirstName = "CHRISTIAN";
            var expectedRecordLastName = "GABLE";
            var expectedRecordLastUpdate = DateTime.Parse("2006-02-15 04:34:33.000");

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testActors = _dvdStoreRepository.GetActors().OrderBy(x => x.Actorid).ToList();
            var actorObjectResult = testActors.FirstOrDefault(x => x.Actorid == expectedRecordActorId);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedRecordActorId, actorObjectResult.Actorid, "Expected ActorId Did NOT Match!");
            Assert.AreEqual(expectedRecordFirstName, actorObjectResult.Firstname, "Expected FirstName Did NOT Match!");
            Assert.AreEqual(expectedRecordLastName, actorObjectResult.Lastname, "Expected LastName Did NOT Match!");
            Assert.AreEqual(expectedRecordLastUpdate, actorObjectResult.Lastupdate, "Expected ActorId Did NOT Match!");
        }

        [TestMethod]
        public void TestActors005_GetActorsPagedList()
        {
            // This is an overly zealous test of PageList of Actors
            // It violates the belief that test should not be complex and ONLY
            // test one thing.  But I'm testing the database sample data
            // another no no so oh well.  :-)
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            // Expected Record to test
            var expectedRecordActorId = 21;
            var expectedRecordFirstName = "KIRSTEN";
            var expectedRecordLastName = "PALTROW";
            var expectedRecordLastUpdate = DateTime.Parse("2006-02-15 04:34:33.000");

            // Expected PagedList Data Items
            var expectedPagedDataCount = 200;
            var expectedPagedDataTotalPages = 10;
            var expectedPagedDataCurrentPage = 2;
            var expectedPageSizeRequested = 20;
            var expectedPagedDataHasNextPage = true;
            var expectedPagedDataHasPreviousPage = true;

            // Arrange the Actor Resource Parameters for the test against the database
            var actorResourceParameters = new ActorResourceParameters();
            actorResourceParameters.PageNumber = 2;
            actorResourceParameters.PageSize = 20;
            // Test OrderBy Options
            actorResourceParameters.OrderBy = "ActorId";

            //*****************************************************************
            // ACT
            //*****************************************************************
            // Grab a list of paged data from the database
            var testActors = _dvdStoreRepository.GetActors(actorResourceParameters);
            // Gather some data about the returned paged list
            var testPagedDataCount = testActors.TotalCount;
            var testPagedDataTotalPages = testActors.TotalPages;
            var testPagedDataCurrentPage = testActors.CurrentPage;
            var testPagedDataPageSize = testActors.PageSize;
            var testPagedDataHasNext = testActors.HasNext;
            var testPagedDataHasPrevious = testActors.HasPrevious;

            // Single out tone from fromthe list to check the data for the this test
            var actorObjectResult = testActors.FirstOrDefault(x => x.Actorid == expectedRecordActorId);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            // Check the PagedList data structure values for this test setup are correct
            Assert.AreEqual(expectedPageSizeRequested, testPagedDataPageSize, $"The expected PagedList Size was not correct.");
            Assert.AreEqual(expectedPagedDataCount, testPagedDataCount, $"The expected PagedList Data Count was not correct.");
            Assert.AreEqual(expectedPagedDataCurrentPage, testPagedDataCurrentPage, $"The expected PagedList Page value was not correct.");
            Assert.AreEqual(expectedPagedDataTotalPages, testPagedDataTotalPages, $"The expected PagedList Total Number of Pages value was not correct.");
            Assert.AreEqual(expectedPagedDataHasNextPage, testPagedDataHasNext, $"The expected PagedList Has Next Page Value was not correct");
            Assert.AreEqual(expectedPagedDataHasPreviousPage, testPagedDataHasPrevious, $"The expected Has Previous Page value was not correct.");

            // Was the singled out record data correct?
            Assert.AreEqual(expectedRecordActorId, actorObjectResult.Actorid, "Expected ActorId Did NOT Match!");
            Assert.AreEqual(expectedRecordFirstName, actorObjectResult.Firstname, "Expected FirstName Did NOT Match!");
            Assert.AreEqual(expectedRecordLastName, actorObjectResult.Lastname, "Expected LastName Did NOT Match!");
            Assert.AreEqual(expectedRecordLastUpdate, actorObjectResult.Lastupdate, "Expected Lastupdate Did NOT Match!");
        }

        [TestMethod]
        public void TestActors006_GetActorByIdWithFilmListing()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var testForActorid = 71;
            var expectedFilmCount = 18;

            //*****************************************************************
            // ACT
            //*****************************************************************
            // Get all the films for and actor by the Actor ID
            var testActorWithFilms = _dvdStoreRepository.GetActorByIdWithFilmListing(testForActorid);
            // Get all the film objects for the actor requested
            var testFilmsDetailsList = testActorWithFilms.Filmactors.Select(x => x.Film);
            // Get one specific expected film object from the list
            var getOneTestRecord = testFilmsDetailsList.FirstOrDefault(x => x.Filmid == 450);
            // Get the count of films for the specific actor with ID #71
            var filmCount = testActorWithFilms.Filmactors.Count;

            // Just dumping the entire list to the debug window.
            // because I like to see them.  :-)
            foreach (var film in testFilmsDetailsList)
            {
                Debug.WriteLine($"{film.Filmid} - {film.Title}");
            }

            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedFilmCount, filmCount, $"Film count doe3s not match!");
            Assert.AreEqual("IDOLS SNATCHERS", getOneTestRecord.Title, $"Title for selected record ID 450 did not match!");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dvdStoreRepository = null;
            _dvdStoreDbContext = null;
        }

        [TestMethod]
        public void TestDBInfo001_GetDatabaseStatistics()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var expectedActorCount = 200;
            var tableName = "actor";

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testResult = _dvdStoreRepository.GetDatabaseStatistics();
            var testActorCountResult = testResult.FirstOrDefault(x => x.TableName == tableName).Rows;
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedActorCount, testActorCountResult, $"Movie title did not match {tableName}");
        }

        [TestMethod]
        public void TestFilms001_GetFilmsIEnumerableList()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var expectedRecordFilmId = 10;
            var expectedRecordTitle = "ALADDIN CALENDAR";
            var expectedRecordDescription = "A Action-Packed Tale of a Man And a Lumberjack who must Reach a Feminist in Ancient China";
            var expectedRecordRentalRate = 4.99m;
            var expectedRecordRating = "NC-17";
            var expectedRecordLastUpdate = DateTime.Parse("2006-02-15 05:03:42.000");

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testFilms = _dvdStoreRepository.GetFilms().OrderBy(x => x.Filmid).ToList();
            var filmObjectResult = testFilms.FirstOrDefault(x => x.Filmid == expectedRecordFilmId);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedRecordFilmId, filmObjectResult.Filmid, "Expected Filmid Did NOT Match!");
            Assert.AreEqual(expectedRecordTitle, filmObjectResult.Title, "Expected Title Did NOT Match!");
            Assert.AreEqual(expectedRecordDescription, filmObjectResult.Description, "Expected Description Did NOT Match!");
            Assert.AreEqual(expectedRecordRentalRate, filmObjectResult.Rentalrate, "Expected Rentalrate Did NOT Match!");
            Assert.AreEqual(expectedRecordRating, filmObjectResult.Rating, "Expected Rating Did NOT Match!");
            Assert.AreEqual(expectedRecordLastUpdate, filmObjectResult.Lastupdate, "Expected Lastupdate Did NOT Match!");
        }

        [TestMethod]
        public void TestFilms002_GetFilmsPagedList()
        {
            // This is an overly zealous test of PageList of Films
            // It violates the belief that test should not be complex and ONLY
            // test one thing.  But I'm testing the database sample data
            // another no no so oh well.  :-)
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            // Expected Record to test
            var expectedRecordFilmId = 21;
            var expectedRecordTitle = "AMERICAN CIRCUS";
            var expectedRecordDescription = "A Insightful Drama of a Girl And a Astronaut who must Face a Database Administrator in A Shark Tank";
            var expectedRecordRentalRate = 4.99m;
            var expectedRecordRating = "R";
            var expectedRecordLastUpdate = DateTime.Parse("2006-02-15 05:03:42.000");

            // Expected PagedList Data Items
            var expectedPagedDataCount = 1000;
            var expectedPagedDataTotalPages = 50;
            var expectedPagedDataCurrentPage = 2;
            var expectedPageSizeRequested = 20;
            var expectedPagedDataHasNextPage = true;
            var expectedPagedDataHasPreviousPage = true;

            // Arrange the Actor Resource Parameters for the test against the database
            var filmResourceParameters = new FilmResourceParameters();
            filmResourceParameters.PageNumber = 2;
            filmResourceParameters.PageSize = 20;
            // Test OrderBy Options
            filmResourceParameters.OrderBy = "Filmid";

            //*****************************************************************
            // ACT
            //*****************************************************************
            // Grab a list of paged data from the database
            var testFilms = _dvdStoreRepository.GetFilms(filmResourceParameters);
            // Gather some data about the returned paged list
            var testPagedDataCount = testFilms.TotalCount;
            var testPagedDataTotalPages = testFilms.TotalPages;
            var testPagedDataCurrentPage = testFilms.CurrentPage;
            var testPagedDataPageSize = testFilms.PageSize;
            var testPagedDataHasNext = testFilms.HasNext;
            var testPagedDataHasPrevious = testFilms.HasPrevious;

            // Single out tone from fromthe list to check the data for the this test
            var filmObjectResult = testFilms.FirstOrDefault(x => x.Filmid == expectedRecordFilmId);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            // Check the PagedList data structure values for this test setup are correct
            Assert.AreEqual(expectedPageSizeRequested, testPagedDataPageSize, $"The expected PagedList Size was not correct.");
            Assert.AreEqual(expectedPagedDataCount, testPagedDataCount, $"The expected PagedList Data Count was not correct.");
            Assert.AreEqual(expectedPagedDataCurrentPage, testPagedDataCurrentPage, $"The expected PagedList Page value was not correct.");
            Assert.AreEqual(expectedPagedDataTotalPages, testPagedDataTotalPages, $"The expected PagedList Total Number of Pages value was not correct.");
            Assert.AreEqual(expectedPagedDataHasNextPage, testPagedDataHasNext, $"The expected PagedList Has Next Page Value was not correct");
            Assert.AreEqual(expectedPagedDataHasPreviousPage, testPagedDataHasPrevious, $"The expected Has Previous Page value was not correct.");

            // Was the singled out record data correct?
            Assert.AreEqual(expectedRecordFilmId, filmObjectResult.Filmid, "Expected Filmid Did NOT Match!");
            Assert.AreEqual(expectedRecordTitle, filmObjectResult.Title, "Expected Title Did NOT Match!");
            Assert.AreEqual(expectedRecordDescription, filmObjectResult.Description, "Expected Description Did NOT Match!");
            Assert.AreEqual(expectedRecordRentalRate, filmObjectResult.Rentalrate, "Expected Rentalrate Did NOT Match!");
            Assert.AreEqual(expectedRecordRating, filmObjectResult.Rating, "Expected Rating Did NOT Match!");
            Assert.AreEqual(expectedRecordLastUpdate, filmObjectResult.Lastupdate, "Expected Lastupdate Did NOT Match!");
        }

        [TestMethod]
        public void TestFilms003_GetFilmsForActorIEnumerableList()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var actorId = 188;
            var expectedRecordFilmId = 10;
            var expectedRecordTitle = "ALADDIN CALENDAR";
            var expectedRecordDescription = "A Action-Packed Tale of a Man And a Lumberjack who must Reach a Feminist in Ancient China";
            var expectedRecordRentalRate = 4.99m;
            var expectedRecordRating = "NC-17";
            var expectedRecordLastUpdate = DateTime.Parse("2006-02-15 05:03:42.000");

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testFilms = _dvdStoreRepository.GetFilms(actorId).OrderBy(x => x.Filmid).ToList();
            var filmObjectResult = testFilms.FirstOrDefault(x => x.Filmid == expectedRecordFilmId);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedRecordFilmId, filmObjectResult.Filmid, "Expected Filmid Did NOT Match!");
            Assert.AreEqual(expectedRecordTitle, filmObjectResult.Title, "Expected Title Did NOT Match!");
            Assert.AreEqual(expectedRecordDescription, filmObjectResult.Description, "Expected Description Did NOT Match!");
            Assert.AreEqual(expectedRecordRentalRate, filmObjectResult.Rentalrate, "Expected Rentalrate Did NOT Match!");
            Assert.AreEqual(expectedRecordRating, filmObjectResult.Rating, "Expected Rating Did NOT Match!");
            Assert.AreEqual(expectedRecordLastUpdate, filmObjectResult.Lastupdate, "Expected Lastupdate Did NOT Match!");
        }

        [TestInitialize]
        public void TestInitialize()
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
            _dvdStoreDbContext = new DVDStoreDBContext(options);

            // Setup repository to test
            _dvdStoreRepository = new DvdStoreRepository(_dvdStoreDbContext);
        }

        #endregion Public Methods
    }
}