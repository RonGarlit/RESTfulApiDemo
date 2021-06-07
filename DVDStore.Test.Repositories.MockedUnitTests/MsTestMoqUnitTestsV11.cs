// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: MsTestMoqUnitTestsV11.cs (DVDStore.Test.Repositories.MockedUnitTests)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  Unit Testing for the repositories.  Using MOQ for Mocking.
// **
// **  Change History
// **
// **  WHEN        WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-11-12  rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

using Api.Helpers;
using DVDStore.Common.Models.v1_1;
using DVDStore.Common.ResourceParameters.v1_1;
using DVDStore.DAL.Models;
using DVDStore.DAL.Repositories.v1_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DVDStore.Test.Repositories.MockedUnitTests
{
    [TestClass]
    public class MsTestMoqUnitTestsV11
    {
        #region Private Fields

        // Create a list of Actors for use in mocking
        private readonly List<Actor> _actorList = new List<Actor>();

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void TestActors001_ActorExists()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var actorToTest = 71;
            var expectedBoolResult = true;
            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.ActorExists(71)).Returns(true);

            //*****************************************************************
            // ACT
            //*****************************************************************
            var doesActorExist = dvdStoreRepositoryMock.Object.ActorExists(actorToTest);

            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedBoolResult, doesActorExist, "Expected actor was not found!");
        }

        [TestMethod]
        public void TestActors001_GetActorListCount()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var expectedValue = 3;
            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetActors()).Returns(_actorList);

            //*****************************************************************
            // ACT
            //*****************************************************************
            var getActorsListCount = dvdStoreRepositoryMock.Object.GetActors().Count();

            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedValue, getActorsListCount, "Expected Actor Count Did NOT Match!");
        }

        [TestMethod]
        public void TestActors002_GetActorById()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var testForActorid = 2;
            var expectedFirstName = "ActorB";
            var expectedLastName = "ActorTwo";

            var actor2 = new DVDStore.DAL.Models.Actor()
            {
                Actorid = 2,
                Lastname = "ActorTwo",
                Firstname = "ActorB"
            };

            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetActorById(testForActorid)).Returns(actor2);
            //*****************************************************************
            // ACT
            //*****************************************************************
            var testForActorTwo = dvdStoreRepositoryMock.Object.GetActorById(testForActorid);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedFirstName, testForActorTwo.Firstname, $"Actor FirstName test Failed!");
            Assert.AreEqual(expectedLastName, testForActorTwo.Lastname, $"Actor LastName test Failed!");
        }

        [TestMethod]
        public void TestActors003_GetActorByFirstNameAndLastName()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var testForActorid = 2;
            var testForActorb = "ActorB";
            var testForActortwo = "ActorTwo";

            var actor2 = new DVDStore.DAL.Models.Actor()
            {
                Actorid = 2,
                Lastname = "ActorTwo",
                Firstname = "ActorB"
            };

            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetActorByFirstNameAndLastName(testForActorb, testForActortwo)).Returns(actor2);

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testActorResult = dvdStoreRepositoryMock.Object.GetActorByFirstNameAndLastName(testForActorb, testForActortwo);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(testForActorid, testActorResult.Actorid, $"Actor Id Test Failed!");
            Assert.AreEqual(testForActorb, testActorResult.Firstname, $"Actor FirstName Test Failed!");
            Assert.AreEqual(testForActortwo, testActorResult.Lastname, $"Actor Lastname Test Failed!");
        }

        [TestMethod]
        public void TestActors004_GetActorFilmListByFirstNameLastName()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************

            var testForActorc = "ActorC";
            var testForActorthree = "ActorThree";
            var testForProgrammersGoneWild = "Programmers Gone Wild";

            var actorFileListing = new List<ActorFilmListing>();

            var actorFilmOne = new ActorFilmListing()
            {
                ActorId = 1,
                LastName = "ActorOne",
                FirstName = "ActorA",
                Rating = "G",
                FilmTitle = "Frozen",
                FilmId = 1
            };
            actorFileListing.Add(actorFilmOne);

            var actorFilmTwo = new ActorFilmListing()
            {
                ActorId = 2,
                LastName = "ActorTwo",
                FirstName = "ActorB",
                Rating = "R",
                FilmTitle = "Ron Garlit The Great!",
                FilmId = 2
            };
            actorFileListing.Add(actorFilmTwo);

            var actorFilmThree = new ActorFilmListing()
            {
                ActorId = 3,
                LastName = "ActorThree",
                FirstName = "ActorC",
                Rating = "NC-17",
                FilmTitle = "Programmers Gone Wild",
                FilmId = 3
            };
            actorFileListing.Add(actorFilmThree);

            var actorFilmFour = new ActorFilmListing()
            {
                ActorId = 4,
                LastName = "ActorFour",
                FirstName = "ActorD",
                Rating = "PG-13",
                FilmTitle = "Mars Attacks the Data Center",
                FilmId = 4
            };
            actorFileListing.Add(actorFilmFour);

            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetActorFilmListByFirstNameLastName(testForActorc, testForActorthree)).Returns(actorFileListing);

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testActorResult =
                dvdStoreRepositoryMock.Object.GetActorFilmListByFirstNameLastName(testForActorc, testForActorthree);
            var testFindMovieTitle = testActorResult.FirstOrDefault(x => x.FilmTitle == testForProgrammersGoneWild).FilmTitle;
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(testForProgrammersGoneWild, testFindMovieTitle, $"Movie title did not match {testForProgrammersGoneWild}");
        }

        [TestMethod]
        public void TestActors005_GetActorsIEnumerableList()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var expectedTenthRecordActorId = 10;
            var expectedTenthRecordFirstName = "CHRISTIAN";
            var expectedTenthRecordLastName = "GABLE";
            // LastUpdate time in the db for this record "2006-02-15 04:34:33.000"
            var expectedTenthRecordLastUpdate = DateTime.Parse("2006-02-15 04:34:33.000");

            var actor09 = new Actor
            {
                Actorid = 09,
                Lastname = "GARLIT",
                Firstname = "RON",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:30.000")
            };

            var actor10 = new Actor
            {
                Actorid = 10,
                Lastname = "GABLE",
                Firstname = "CHRISTIAN",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:33.000")
            };

            var actor11 = new Actor
            {
                Actorid = 10,
                Lastname = "MOLINA",
                Firstname = "BORIS",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:40.000")
            };

            var actorList = new List<Actor>
            {
                actor09,
                actor10,
                actor11
            };

            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetActors()).Returns(actorList);

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testActors = dvdStoreRepositoryMock.Object.GetActors();
            var actorObjectResult = testActors.FirstOrDefault(x => x.Actorid == expectedTenthRecordActorId);
            //*****************************************************************
            // ASSERT
            //*****************************************************************
            Assert.AreEqual(expectedTenthRecordActorId, actorObjectResult.Actorid, "Expected ActorId Did NOT Match!");
            Assert.AreEqual(expectedTenthRecordFirstName, actorObjectResult.Firstname, "Expected FirstName Did NOT Match!");
            Assert.AreEqual(expectedTenthRecordLastName, actorObjectResult.Lastname, "Expected LastName Did NOT Match!");
            Assert.AreEqual(expectedTenthRecordLastUpdate, actorObjectResult.Lastupdate, "Expected ActorId Did NOT Match!");
        }

        [TestMethod]
        public void TestActors006_GetActorsPagedList()
        {
            // This is an overly zealous test of PageList of Actors
            // It violates the belief that test should not be complex and ONLY
            // test one thing.  But I'm testing the database sample data
            // another no no so oh well.  :-)
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            // Expected Record to test
            var expectedTenthRecordActorId = 3;
            var expectedTenthRecordFirstName = "Actor03FirstName";
            var expectedTenthRecordLastName = "Actor03Lastname";
            var expectedTenthRecordLastUpdate = DateTime.Parse("2006-02-15 04:34:30.000");

            // Expected PagedList Data Items
            var expectedPagedDataCount = 6;
            var expectedPagedDataTotalPages = 3;
            var expectedPagedDataCurrentPage = 2;
            var expectedPageSizeRequested = 2;
            var expectedPagedDataHasNextPage = true;
            var expectedPagedDataHasPreviousPage = true;

            // Arrange the Actor Resource Parameters for the test against the database
            var actorResourceParameters = new ActorResourceParameters
            {
                PageNumber = 2,
                PageSize = 2,
                // Test OrderBy Options
                OrderBy = "ActorId"
            };

            // Mock Actor Data
            var actor01 = new Actor
            {
                Actorid = 01,
                Lastname = "Actor01Lastname",
                Firstname = "Actor01FirstName",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:10.000")
            };

            var actor02 = new Actor
            {
                Actorid = 02,
                Lastname = "Actor02Lastname",
                Firstname = "Actor02FirstName",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:20.000")
            };

            var actor03 = new Actor
            {
                Actorid = 03,
                Lastname = "Actor03Lastname",
                Firstname = "Actor03FirstName",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:30.000")
            };
            var actor04 = new Actor
            {
                Actorid = 04,
                Lastname = "Actor04Lastname",
                Firstname = "Actor04FirstName",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:40.000")
            };

            var actor05 = new Actor
            {
                Actorid = 05,
                Lastname = "Actor05Lastname",
                Firstname = "Actor05FirstName",
                Lastupdate = DateTime.Parse("2006-02-15 04:34:50.000")
            };

            var actor06 = new Actor
            {
                Actorid = 06,
                Lastname = "Actor06Lastname",
                Firstname = "Actor06FirstName",
                Lastupdate = DateTime.Parse("2006-02-15 04:35:00.000")
            };

            var actorList = new List<Actor>
            {
                actor01,
                actor02,
                actor03,
                actor04,
                actor05,
                actor06
            };

            // Setup Paged list for Mock with the List of Actors from above
            // with the count, page number and page size we are asking for
            // in the test
            PagedList<Actor> listOfActor = new PagedList<Actor>(actorList, 6, 2, 2);

            // Create the Mock Repository Object
            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetActors(actorResourceParameters)).Returns(listOfActor);

            //*****************************************************************
            // ACT
            //*****************************************************************
            // Grab a list of paged data from the database
            var testActors = dvdStoreRepositoryMock.Object.GetActors(actorResourceParameters);
            // Gather some data about the returned paged list
            var testPagedDataCount = testActors.TotalCount;
            var testPagedDataTotalPages = testActors.TotalPages;
            var testPagedDataCurrentPage = testActors.CurrentPage;
            var testPagedDataPageSize = testActors.PageSize;
            var testPagedDataHasNext = testActors.HasNext;
            var testPagedDataHasPrevious = testActors.HasPrevious;

            // Single out tone from fromthe list to check the data for the this test
            var actorObjectResult = testActors.FirstOrDefault(x => x.Actorid == expectedTenthRecordActorId);
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
            Assert.AreEqual(expectedTenthRecordActorId, actorObjectResult.Actorid, "Expected ActorId Did NOT Match!");
            Assert.AreEqual(expectedTenthRecordFirstName, actorObjectResult.Firstname, "Expected FirstName Did NOT Match!");
            Assert.AreEqual(expectedTenthRecordLastName, actorObjectResult.Lastname, "Expected LastName Did NOT Match!");
            Assert.AreEqual(expectedTenthRecordLastUpdate, actorObjectResult.Lastupdate, "Expected ActorId Did NOT Match!");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _actorList.Clear();
        }

        [TestMethod]
        public void TestDBInfo001_GetDatabaseStatistics()
        {
            //*****************************************************************
            // ARRANGE
            //*****************************************************************
            var expectedActorCount = 200;
            var tableName = "actor";

            var dbStatList = new List<UspGetDatabaseStatisticsReturnModel>();

            var actorTableStats = new UspGetDatabaseStatisticsReturnModel
            {
                DataSpaceUsed = "8 KB",
                SpaceReservedUsed = "144 KB",
                IndexSpaceUsed = "0 KB",
                TableName = "actor",
                Rows = 200,
                UnusedSpace = "112 KB"
            };

            dbStatList.Add(actorTableStats);

            var filmTableStats = new UspGetDatabaseStatisticsReturnModel
            {
                DataSpaceUsed = "10 KB",
                SpaceReservedUsed = "200 KB",
                IndexSpaceUsed = "10 KB",
                TableName = "film",
                Rows = 1000,
                UnusedSpace = "333 KB"
            };

            dbStatList.Add(filmTableStats);

            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetDatabaseStatistics()).Returns(dbStatList);

            //*****************************************************************
            // ACT
            //*****************************************************************
            var testResult = dvdStoreRepositoryMock.Object.GetDatabaseStatistics();
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
            var expectedRecordFilmId = 3;
            var expectedRecordTitle = "film03";
            var expectedRecordDescription = "Test of film03";
            var expectedRecordRentalRate = 3.99m;
            var expectedRecordRating = "NC-17";
            var expectedRecordLastUpdate = DateTime.Parse("2006-02-15 03:03:42.000");

            var film01 = new Film()
            {
                Filmid = 1,
                Title = "film01",
                Description = "Test of film01",
                Rentalrate = 1.99m,
                Rating = "G",
                Lastupdate = DateTime.Parse("2006-02-15 01:03:42.000")
            };

            var film02 = new Film()
            {
                Filmid = 2,
                Title = "film02",
                Description = "Test of film02",
                Rentalrate = 2.99m,
                Rating = "R",
                Lastupdate = DateTime.Parse("2006-02-15 02:03:42.000")
            };

            var film03 = new Film()
            {
                Filmid = 3,
                Title = "film03",
                Description = "Test of film03",
                Rentalrate = 3.99m,
                Rating = "NC-17",
                Lastupdate = DateTime.Parse("2006-02-15 03:03:42.000")
            };

            var film04 = new Film()
            {
                Filmid = 4,
                Title = "film04",
                Description = "Test of film04",
                Rentalrate = 4.99m,
                Rating = "PG-13",
                Lastupdate = DateTime.Parse("2006-02-15 04:03:42.000")
            };

            var film05 = new Film()
            {
                Filmid = 5,
                Title = "film05",
                Description = "Test of film05",
                Rentalrate = 5.99m,
                Rating = "PG",
                Lastupdate = DateTime.Parse("2006-02-15 05:03:42.000")
            };

            var film06 = new Film()
            {
                Filmid = 6,
                Title = "film06",
                Description = "Test of film06",
                Rentalrate = 6.99m,
                Rating = "NC-17",
                Lastupdate = DateTime.Parse("2006-02-15 06:03:42.000")
            };

            var filmList = new List<Film>()
            {
                film01,
                film02,
                film03,
                film04,
                film05,
                film06
            };

            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetFilms()).Returns(filmList);

            //*****************************************************************
            // ACT
            //*****************************************************************

            var testFilms = dvdStoreRepositoryMock.Object.GetFilms().OrderBy(x => x.Filmid).ToList();
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
            var expectedRecordFilmId = 3;
            var expectedRecordTitle = "film03";
            var expectedRecordDescription = "Test of film03";
            var expectedRecordRentalRate = 3.99m;
            var expectedRecordRating = "NC-17";
            var expectedRecordLastUpdate = DateTime.Parse("2006-02-15 03:03:42.000");

            var film01 = new Film()
            {
                Filmid = 1,
                Title = "film01",
                Description = "Test of film01",
                Rentalrate = 1.99m,
                Rating = "G",
                Lastupdate = DateTime.Parse("2006-02-15 01:03:42.000")
            };

            var film02 = new Film()
            {
                Filmid = 2,
                Title = "film02",
                Description = "Test of film02",
                Rentalrate = 2.99m,
                Rating = "R",
                Lastupdate = DateTime.Parse("2006-02-15 02:03:42.000")
            };

            var film03 = new Film()
            {
                Filmid = 3,
                Title = "film03",
                Description = "Test of film03",
                Rentalrate = 3.99m,
                Rating = "NC-17",
                Lastupdate = DateTime.Parse("2006-02-15 03:03:42.000")
            };

            var film04 = new Film()
            {
                Filmid = 4,
                Title = "film04",
                Description = "Test of film04",
                Rentalrate = 4.99m,
                Rating = "PG-13",
                Lastupdate = DateTime.Parse("2006-02-15 04:03:42.000")
            };

            var film05 = new Film()
            {
                Filmid = 5,
                Title = "film05",
                Description = "Test of film05",
                Rentalrate = 5.99m,
                Rating = "PG",
                Lastupdate = DateTime.Parse("2006-02-15 05:03:42.000")
            };

            var film06 = new Film()
            {
                Filmid = 6,
                Title = "film06",
                Description = "Test of film06",
                Rentalrate = 6.99m,
                Rating = "NC-17",
                Lastupdate = DateTime.Parse("2006-02-15 06:03:42.000")
            };

            var filmList = new List<Film>()
            {
                film01,
                film02,
                film03,
                film04,
                film05,
                film06
            };

            // Expected PagedList Data Items
            var expectedPagedDataCount = 6;
            var expectedPagedDataTotalPages = 3;
            var expectedPagedDataCurrentPage = 2;
            var expectedPageSizeRequested = 2;
            var expectedPagedDataHasNextPage = true;
            var expectedPagedDataHasPreviousPage = true;

            // Arrange the Film Resource Parameters for the test against the database
            var filmResourceParameters = new FilmResourceParameters
            {
                PageNumber = 2,
                PageSize = 2,
                // Test OrderBy Options
                OrderBy = "Filmid"
            };

            // Setup Paged list for Mock with the List of Films from above
            // with the count, page number and page size we are asking for
            // in the test
            PagedList<Film> listOfFilms = new PagedList<Film>(filmList, 6, 2, 2);

            var dvdStoreRepositoryMock = new Mock<IDvdStoreRepository>();
            dvdStoreRepositoryMock.Setup(repo => repo.GetFilms(filmResourceParameters)).Returns(listOfFilms);

            //*****************************************************************
            // ACT
            //*****************************************************************
            // Grab a list of paged data from the database
            var testFilms = dvdStoreRepositoryMock.Object.GetFilms(filmResourceParameters);
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

        [TestInitialize]
        public void TestInitialize()
        {
            // Lost the _actorList with data used for m
            var actor1 = new Actor
            {
                Actorid = 1,
                Lastname = "ActorOne",
                Firstname = "ActorA"
            };

            _actorList.Add(actor1);

            var actor2 = new Actor
            {
                Actorid = 2,
                Lastname = "ActorTwo",
                Firstname = "ActorB"
            };
            _actorList.Add(actor2);

            var actor3 = new Actor
            {
                Actorid = 3,
                Lastname = "ActorThree",
                Firstname = "ActorC"
            };
            _actorList.Add(actor3);
        }

        #endregion Public Methods
    }
}