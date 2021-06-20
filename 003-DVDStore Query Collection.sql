/***************************************************************************************************
FILENAME: 003-DVDStore Query Collection
***************************************************************************************************/

/***************************************************************************************************
NOTES: A bunch of queries of the DVDStore database
***************************************************************************************************/


USE DVDStore;
--Find out how much data is in the database

EXEC sp_MSforeachtable 
     @precommand = 'CREATE TABLE ##spaceused 
        (TableName varchar(128) NOT NULL, 
        Rows int,
        SpaceReservedUsed varchar(200),
        DataSpaceUsed varchar(200),
        IndexSpaceUsed varchar(200),
        UnusedSpace varchar(200))'
   , @command1 = "INSERT INTO ##spaceused (TableName, Rows, SpaceReservedUsed, DataSpaceUsed, IndexSpaceUsed, UnusedSpace) EXEC sp_spaceused '?'"
   , @postcommand = 'SELECT *
            FROM ##spaceused
            ORDER BY Rows DESC;
        DROP TABLE ##spaceused';
-- Display the first and last names of all actors from the table actor.

SELECT firstname
     , lastname
FROM   actor;
GO
-- Display the first and last name of each actor in a single column in upper case letters. Name the column Actor Name.

SELECT UPPER(CONCAT(firstname, ' ', lastname))
       AS Actor_Name
FROM   actor;
GO
-- You need to find the ID number, first name, and last name of an actor, of whom you know only the first name, 'Joe.' 

SELECT actorid
     , firstname
     , lastname
FROM   actor
WHERE  firstname = 'Joe';
GO
-- Find all actors whose last name contain the letters GEN:

SELECT *
FROM   actor
WHERE  lastname LIKE '%GEN%';
GO
-- Find all actors whose last names contain the letters LI. 
-- This time, order the rows by last name and first name, in that order:

SELECT lastname
     , firstname
FROM   actor
WHERE  lastname LIKE '%LI%';
GO
-- Using IN, display the countryid and country columns of the following countries: Afghanistan, Bangladesh, and China:

SELECT countryid
     , country
FROM   country
WHERE  country IN
                 (
                  'Afghanistan'
                , 'Bangladesh'
                , 'China'
                 );
GO
-- List the last names of actors, as well as how many actors have that last name.

SELECT lastname
     , COUNT(lastname)
       AS 'Number_of_Actors'
FROM   actor
GROUP BY lastname;
GO
-- List last names of actors and the number of actors who have that last name, 
-- but only for names that are shared by at least two actors

SELECT lastname
     , COUNT(lastname)
       AS Number_of_Actors
FROM   actor
GROUP BY lastname
HAVING COUNT(lastname) > 1;
GO
-- Oh, no! The actor HARPO WILLIAMS was accidentally entered in the actor table as GROUCHO WILLIAMS, 
-- the name of Harpo's second cousin's husband's yoga teacher. Write a query to fix the record.
--UPDATE actor
--SET firstname = 'HARPO'
--WHERE actorid = 172;
-- In a single query, if the first name of the actor is currently HARPO, change it to GROUCHO. 
--UPDATE actor
--SET firstname = 'GROUCHO'
--WHERE actorid = 172;
-- Explore all the tables and their columns with data typeinformation in the database

SELECT @@servername
       AS SERVER
     , DB_NAME()
       AS dbname
     , isc.table_name
       AS tablename
     , isc.table_schema
       AS schemaname
     , ordinal_position
       AS ord
     , column_name
     , data_type
     , numeric_precision
       AS prec
     , numeric_scale
       AS scale
     , character_maximum_length
       AS len -- -1 means max like varchar(max)   
     , is_nullable
     , column_default
     , table_type
FROM   INFORMATION_SCHEMA.COLUMNS
     AS isc
       INNER JOIN INFORMATION_SCHEMA.TABLES
     AS ist ON isc.table_name = ist.table_name
--      where table_type = 'base table' -- 'base table' or 'view' 
ORDER BY dbname
       , tablename
       , schemaname
       , ordinal_position;
GO
-- Use JOIN to display the first and last names, as well as the address, of each staff member. 
-- Use the tables staff and address:

SELECT s.firstname
     , s.lastname
     , a.address
FROM   staff
     AS s
       JOIN address
     AS a ON s.addressid = a.addressid;
GO
-- Use JOIN to display the total amount rung up by each staff member in August of 2005. Use tables staff and payment

SELECT s.staffid
     , SUM(p.amount)
       AS 'August_2005_amount'
FROM   staff
     AS s
       INNER JOIN payment
     AS p ON s.staffid = p.staffid
WHERE  CONVERT(DATE, p.paymentdate) LIKE '2005-08%'
GROUP BY s.staffid;
GO
-- List each film and the number of actors who are listed for that film. Use tables filmactor and film. Use inner join.

SELECT fa.filmid
     , COUNT(fa.actorid)
       AS 'Number_of_Actors'
FROM   filmactor
     AS fa
       INNER JOIN film
     AS f ON fa.filmid = f.filmid
GROUP BY fa.filmid;
GO
-- How many copies of the film Hunchback Impossible exist in the inventory system?

SELECT filmid
     , COUNT(filmid)
       AS 'Number_of_Copies'
FROM   inventory
WHERE  filmid =
      (
        SELECT filmid
        FROM   film
        WHERE  title = 'HUNCHBACK IMPOSSIBLE'
      )
GROUP BY filmid;
GO
-- Using the tables payment and customer and the JOIN command, list the total paid by each customer. 
-- List the customers alphabetically by last name:

SELECT customer.firstname
     , customer.lastname
     , SUM(payment.amount)
       AS 'Total Amount Paid'
FROM   payment
       JOIN customer ON payment.customerid = customer.customerid
GROUP BY lastname
       , firstname
ORDER BY lastname ASC;
GO
-- Use subqueries to display all actors who appear in the film Alone Trip.

SELECT actor.firstname
     , actor.lastname
FROM   actor
WHERE  actorid IN
                   (
                     SELECT actorid
                     FROM   filmactor
                     WHERE  filmid =
                           (
                             SELECT filmid
                             FROM   film
                             WHERE  title = 'ALONE TRIP'
                           )
                   );
GO
-- You want to run an email marketing campaign in Canada, for which you will need the names and email addresses of all Canadian customers.
-- Use joins to retrieve this information.

SELECT customer.firstname
     , customer.lastname
     , customer.email
FROM   country
       INNER JOIN city ON country.countryid = city.countryid
       INNER JOIN address ON address.cityid = city.cityid
       INNER JOIN customer ON customer.addressid = address.addressid
WHERE  city.countryid = 20;
GO
-- Sales have been lagging among young families, and you wish to target all family movies for a promotion. 
-- Identify all movies categorized as famiy films.

SELECT film.title
FROM   category
       INNER JOIN filmcategory ON category.categoryid = filmcategory.categoryid
       INNER JOIN film ON film.filmid = filmcategory.filmid
WHERE  category.NAME = 'Family';
GO
-- Display the most frequently rented movies in descending order.

SELECT film.title
     , COUNT(film.title)
       AS 'Number_of_Rentals'
FROM   rental
       INNER JOIN inventory ON inventory.filmid = rental.inventoryid
       INNER JOIN film ON film.filmid = inventory.filmid
GROUP BY title
ORDER BY Number_of_Rentals DESC;
GO
-- Write a query to display how much business, in dollars, each store brought in.

SELECT store.storeid
     , FORMAT(SUM(amount), 'C', 'en-US')
       AS 'Revenue'
FROM   store
       INNER JOIN staff ON store.storeid = staff.storeid
       INNER JOIN rental ON rental.staffid = staff.staffid
       INNER JOIN payment ON payment.rentalid = rental.rentalid
GROUP BY store.storeid;
GO
-- Write a query to display for each store its store ID, city, and country.

SELECT store.storeid
     , city.city
     , country.country
FROM   store
       INNER JOIN address ON store.addressid = address.addressid
       INNER JOIN city ON city.cityid = address.cityid
       INNER JOIN country ON country.countryid = city.countryid;
GO
-- List the top five genres in gross revenue in descending order. 

SELECT TOP 5 category.NAME
           , SUM(payment.amount)
       AS 'Gross_Revenue'
FROM         category
             INNER JOIN filmcategory ON category.categoryid = filmcategory.categoryid
             INNER JOIN inventory ON inventory.filmid = filmcategory.filmid
             INNER JOIN rental ON rental.inventoryid = inventory.inventoryid
             INNER JOIN payment ON payment.rentalid = rental.rentalid
GROUP BY category.NAME
ORDER BY Gross_Revenue DESC;
GO
-- In your new role as an executive, you would like to have an easy way of viewing the Top five genres by gross revenue. 
-- Use the solution from the problem above to create a view. If you haven't solved 7h, you can substitute another query to create a view.

CREATE VIEW dbo.top_five_genres
AS
     SELECT TOP 5 category.NAME
                , SUM(payment.amount)
            AS 'Gross_Revenue'
     FROM         category
                  INNER JOIN filmcategory ON category.categoryid = filmcategory.categoryid
                  INNER JOIN inventory ON inventory.filmid = filmcategory.filmid
                  INNER JOIN rental ON rental.inventoryid = inventory.inventoryid
                  INNER JOIN payment ON payment.rentalid = rental.rentalid
     GROUP BY category.NAME;
GO
-- How would you display the view that you created in 8a?

SELECT *
FROM   top_five_genres;
GO
-- You find that you no longer need the view top_five_genres. Write a query to delete it.

DROP VIEW top_five_genres;
GO