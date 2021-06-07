// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: DVDStoreDBContext.cs (DVDStoreDbLibrary)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  This was initially built using the dotnet ef dbcontext scaffold command.
// **  It is the starting base for EF development beside my preferred method of
// **  the Reverse POCO Generator T4 code which is no longer open source but a
// **  paid product.  But is VERY worth the expense giving us more features in
// **  second that I could code in days like I'm doing here.
// **
// **  Change History
// **
// **  WHEN         WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-10-27   rgarlit     STARTED DEVELOPMENT
// **  2020-11-16   rgarlit     Revised for EF Core 5.0 - with Many to many changes
// ***********************************************************************************/

using DVDStore.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace DVDStore.DAL.Context
{
    public partial class DVDStoreDBContext : DbContext, IDVDStoreDBContext
    {
        #region Public Constructors

        public DVDStoreDBContext()
        {
        }

        public DVDStoreDBContext(DbContextOptions<DVDStoreDBContext> options)
            : base(options)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customerlist> Customerlists { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Filmactor> Filmactors { get; set; }
        public virtual DbSet<Filmcategory> Filmcategories { get; set; }
        public virtual DbSet<Filmlist> Filmlists { get; set; }
        public virtual DbSet<FilmRev> FilmRevs { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Filmtext> Filmtexts { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Rental> Rentals { get; set; }
        public virtual DbSet<Salesbyfilmcategory> Salesbyfilmcategories { get; set; }
        public virtual DbSet<Salesbystore> Salesbystores { get; set; }
        public virtual DbSet<staff> staff { get; set; }
        public virtual DbSet<Stafflist> Stafflists { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<TempActorFilmListing> TempActorFilmListings { get; set; }

        #endregion Public Properties

        #region Public Methods

        public List<UspGetDatabaseStatisticsReturnModel> UspGetDatabaseStatistics()
        {
            int procResult;
            return UspGetDatabaseStatistics(out procResult);
        }

        // FYI: Manually added for stored procedures
        //=====================================================================
        // Stored Procedures
        //=====================================================================
        // This coded is based on discussion with Simon Hughes on how to is
        // the best way to handle stored procedures in EF Core and emulates that.
        //=====================================================================
        // This involved using the Set Method on the DbContext in conjunction
        // with at "RETURN MODEL" created to catch the data from the stored procedure.
        // CREF="https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext.set?view=efcore-3.1"
        // When the SP is NOT returning a model that is related to an already
        // created DbSet<T> method you need to create a model class for that.
        // As shown in this C-Sharp Corner article owned by my old buddy Mahesh Chand.
        // CREF="https://www.c-sharpcorner.com/article/asp-net-core-entity-framework-call-store-procedure/"
        //=====================================================================
        public List<UspGetDatabaseStatisticsReturnModel> UspGetDatabaseStatistics(out int procResult)
        {
            var procResultParam = new SqlParameter
            { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            const string sqlCommand = "EXEC @procResult = [dbo].[usp_GetDatabaseStatistics]";
            var procResultData = Set<UspGetDatabaseStatisticsReturnModel>()
                .FromSqlRaw(sqlCommand, procResultParam)
                .ToList();

            procResult = (int)procResultParam.Value;
            return procResultData;
        }

        public async Task<List<UspGetDatabaseStatisticsReturnModel>> UspGetDatabaseStatisticsAsync()
        {
            const string sqlCommand = "EXEC [dbo].[usp_GetDatabaseStatistics]";
            var procResultData = await Set<UspGetDatabaseStatisticsReturnModel>()
                .FromSqlRaw(sqlCommand)
                .ToListAsync();

            return procResultData;
        }

        public List<UspGetListOfTitlesByCategoryReturnModel> UspGetListOfTitlesByCategory(string categoryName)
        {
            int procResult;
            return UspGetListOfTitlesByCategory(categoryName, out procResult);
        }

        public List<UspGetListOfTitlesByCategoryReturnModel> UspGetListOfTitlesByCategory(string categoryName,
            out int procResult)
        {
            var categoryNameParam = new SqlParameter
            {
                ParameterName = "@categoryName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = categoryName,
                Size = 25
            };
            if (categoryNameParam.Value == null)
            {
                categoryNameParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter
            { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            const string sqlCommand = "EXEC @procResult = [dbo].[usp_GetListOfTitlesByCategory] @categoryName";
            var procResultData = Set<UspGetListOfTitlesByCategoryReturnModel>()
                .FromSqlRaw(sqlCommand, categoryNameParam, procResultParam)
                .ToList();

            procResult = (int)procResultParam.Value;
            return procResultData;
        }

        public async Task<List<UspGetListOfTitlesByCategoryReturnModel>> UspGetListOfTitlesByCategoryAsync(
            string categoryName)
        {
            var categoryNameParam = new SqlParameter
            {
                ParameterName = "@categoryName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = categoryName,
                Size = 25
            };
            if (categoryNameParam.Value == null)
            {
                categoryNameParam.Value = DBNull.Value;
            }

            const string sqlCommand = "EXEC [dbo].[usp_GetListOfTitlesByCategory] @categoryName";
            var procResultData = await Set<UspGetListOfTitlesByCategoryReturnModel>()
                .FromSqlRaw(sqlCommand, categoryNameParam)
                .ToListAsync();

            return procResultData;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //                optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=DVDStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        /// <summary>
        ///     OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <remarks>
        ///     Entity Framework uses a set of conventions to build a model based
        ///     on the shape of your entity classes. You can specify additional
        ///     configuration to supplement and/or override what was discovered
        ///     by convention.The configuration that can be applied to a model
        ///     targeting any data store and that which can be applied when
        ///     targeting any relational database.
        /// </remarks>
        /// <see cref="https://docs.microsoft.com/en-us/ef/core/modeling/" />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actor");

                entity.HasIndex(e => e.Lastname, "idxactorlastname");

                entity.Property(e => e.Actorid).HasColumnName("actorid");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.HasIndex(e => e.Cityid, "idxfkcityid");

                entity.Property(e => e.Addressid).HasColumnName("addressid");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address2");

                entity.Property(e => e.Cityid).HasColumnName("cityid");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("district");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Postalcode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("postalcode");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.Cityid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkaddresscity");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Categoryid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("categoryid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.HasIndex(e => e.Countryid, "idxfkcountryid");

                entity.Property(e => e.Cityid).HasColumnName("cityid");

                entity.Property(e => e.City1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Countryid).HasColumnName("countryid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkcitycountry");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.Property(e => e.Countryid).HasColumnName("countryid");

                entity.Property(e => e.Country1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.Addressid, "idxfkaddressid");

                entity.HasIndex(e => e.Storeid, "idxfkstoreid");

                entity.HasIndex(e => e.Lastname, "idxlastname");

                entity.Property(e => e.Customerid).HasColumnName("customerid");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.Addressid).HasColumnName("addressid");

                entity.Property(e => e.Createdate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.Addressid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkcustomeraddress");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.Storeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkcustomerstore");
            });

            modelBuilder.Entity<Customerlist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("customerlist");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(91)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("notes");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Sid).HasColumnName("SID");

                entity.Property(e => e.Zipcode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("zipcode");
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("film");

                entity.HasIndex(e => e.Languageid, "idxfklanguageid");

                entity.HasIndex(e => e.Originallanguageid, "idxfkoriginallanguageid");

                entity.Property(e => e.Filmid).HasColumnName("filmid");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Languageid).HasColumnName("languageid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.Property(e => e.Originallanguageid).HasColumnName("originallanguageid");

                entity.Property(e => e.Rating)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("rating")
                    .HasDefaultValueSql("('G')");

                entity.Property(e => e.Releaseyear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("releaseyear");

                entity.Property(e => e.Rentalduration)
                    .HasColumnName("rentalduration")
                    .HasDefaultValueSql("((3))");

                entity.Property(e => e.Rentalrate)
                    .HasColumnType("decimal(4, 2)")
                    .HasColumnName("rentalrate")
                    .HasDefaultValueSql("((4.99))");

                entity.Property(e => e.Replacementcost)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("replacementcost")
                    .HasDefaultValueSql("((19.99))");

                entity.Property(e => e.Specialfeatures)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("specialfeatures");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.FilmLanguages)
                    .HasForeignKey(d => d.Languageid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkfilmlanguage");

                entity.HasOne(d => d.Originallanguage)
                    .WithMany(p => p.FilmOriginallanguages)
                    .HasForeignKey(d => d.Originallanguageid)
                    .HasConstraintName("fkfilmlanguageoriginal");
            });

            modelBuilder.Entity<FilmRev>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("filmRev");

                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.Filmid).HasColumnName("filmid");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Originallanguageid).HasColumnName("originallanguageid");

                entity.Property(e => e.Rating)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("rating");

                entity.Property(e => e.Rentalduration).HasColumnName("rentalduration");

                entity.Property(e => e.Rentalrate)
                    .HasColumnType("decimal(4, 2)")
                    .HasColumnName("rentalrate");

                entity.Property(e => e.Replacementcost)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("replacementcost");

                entity.Property(e => e.Specialfeatures)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("specialfeatures");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Filmactor>(entity =>
            {
                entity.HasKey(e => new { e.Actorid, e.Filmid })
                    .HasName("PK__filmacto__0F30213FE619491A");

                entity.ToTable("filmactor");

                entity.HasIndex(e => e.Actorid, "idxfkfilmactoractor");

                entity.HasIndex(e => e.Filmid, "idxfkfilmactorfilm");

                entity.Property(e => e.Actorid).HasColumnName("actorid");

                entity.Property(e => e.Filmid).HasColumnName("filmid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.Filmactors)
                    .HasForeignKey(d => d.Actorid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkfilmactoractor");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Filmactors)
                    .HasForeignKey(d => d.Filmid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkfilmactorfilm");
            });

            modelBuilder.Entity<Filmcategory>(entity =>
            {
                entity.HasKey(e => new { e.Filmid, e.Categoryid })
                    .HasName("PK__filmcate__D20B1E904C19ED64");

                entity.ToTable("filmcategory");

                entity.HasIndex(e => e.Categoryid, "idxfkfilmcategorycategory");

                entity.HasIndex(e => e.Filmid, "idxfkfilmcategoryfilm");

                entity.Property(e => e.Filmid).HasColumnName("filmid");

                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Filmcategories)
                    .HasForeignKey(d => d.Categoryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkfilmcategorycategory");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Filmcategories)
                    .HasForeignKey(d => d.Filmid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkfilmcategoryfilm");
            });

            modelBuilder.Entity<Filmlist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("filmlist");

                entity.Property(e => e.Actors)
                    .IsRequired()
                    .HasMaxLength(91)
                    .IsUnicode(false)
                    .HasColumnName("actors");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("category");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Fid).HasColumnName("FID");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(4, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Rating)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("rating");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Filmtext>(entity =>
            {
                entity.HasKey(e => e.Filmid)
                    .HasName("PK__filmtext__C037C0C9146603EE");

                entity.ToTable("filmtext");

                entity.Property(e => e.Filmid)
                    .ValueGeneratedNever()
                    .HasColumnName("filmid");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("inventory");

                entity.HasIndex(e => e.Filmid, "idxfkfilmid");

                entity.HasIndex(e => new { e.Storeid, e.Filmid }, "idxfkfilmidstoreid");

                entity.Property(e => e.Inventoryid).HasColumnName("inventoryid");

                entity.Property(e => e.Filmid).HasColumnName("filmid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.Filmid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkinventoryfilm");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.Storeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkinventorystore");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("language");

                entity.Property(e => e.Languageid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("languageid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.HasIndex(e => e.Customerid, "idxfkcustomerid");

                entity.HasIndex(e => e.Staffid, "idxfkstaffid");

                entity.Property(e => e.Paymentid).HasColumnName("paymentid");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Customerid).HasColumnName("customerid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Paymentdate)
                    .HasColumnType("datetime")
                    .HasColumnName("paymentdate");

                entity.Property(e => e.Rentalid).HasColumnName("rentalid");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Customerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkpaymentcustomer");

                entity.HasOne(d => d.Rental)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Rentalid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fkpaymentrental");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Staffid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkpaymentstaff");
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.ToTable("rental");

                entity.HasIndex(e => e.Customerid, "idxfkcustomerid");

                entity.HasIndex(e => e.Inventoryid, "idxfkinventoryid");

                entity.HasIndex(e => e.Staffid, "idxfkstaffid");

                entity.HasIndex(e => new { e.Rentaldate, e.Inventoryid, e.Customerid }, "idxuq")
                    .IsUnique();

                entity.Property(e => e.Rentalid).HasColumnName("rentalid");

                entity.Property(e => e.Customerid).HasColumnName("customerid");

                entity.Property(e => e.Inventoryid).HasColumnName("inventoryid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rentaldate)
                    .HasColumnType("datetime")
                    .HasColumnName("rentaldate");

                entity.Property(e => e.Returndate)
                    .HasColumnType("datetime")
                    .HasColumnName("returndate");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.Customerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkrentalcustomer");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.Inventoryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkrentalinventory");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.Staffid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkrentalstaff");
            });

            modelBuilder.Entity<Salesbyfilmcategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("salesbyfilmcategory");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("category");

                entity.Property(e => e.Totalsales)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("totalsales");
            });

            modelBuilder.Entity<Salesbystore>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("salesbystore");

                entity.Property(e => e.Manager)
                    .IsRequired()
                    .HasMaxLength(91)
                    .IsUnicode(false)
                    .HasColumnName("manager");

                entity.Property(e => e.Store)
                    .IsRequired()
                    .HasMaxLength(101)
                    .IsUnicode(false)
                    .HasColumnName("store");

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.Totalsales)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("totalsales");
            });

            modelBuilder.Entity<Stafflist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("stafflist");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(91)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Sid).HasColumnName("SID");

                entity.Property(e => e.Zipcode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("zipcode");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("store");

                entity.HasIndex(e => e.Managerstaffid, "idxfkaddressid")
                    .IsUnique();

                entity.HasIndex(e => e.Addressid, "idxfkstoreaddress");

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.Addressid).HasColumnName("addressid");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Managerstaffid).HasColumnName("managerstaffid");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.Addressid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkstoreaddress");

                entity.HasOne(d => d.Managerstaff)
                    .WithOne(p => p.Store)
                    .HasForeignKey<Store>(d => d.Managerstaffid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkstorestaff");
            });

            modelBuilder.Entity<TempActorFilmListing>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tempActorFilmListing");

                entity.Property(e => e.FilmTitle)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Rating)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.HasIndex(e => e.Addressid, "idxfkaddressid");

                entity.HasIndex(e => e.Storeid, "idxfkstoreid");

                entity.Property(e => e.Staffid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("staffid");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Addressid).HasColumnName("addressid");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Lastupdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastupdate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Picture)
                    .HasColumnType("image")
                    .HasColumnName("picture");

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.Addressid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkstaffaddress");

                entity.HasOne(d => d.StoreNavigation)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.Storeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkstaffstore");
            });

            //=================================================================
            // FYI: Important Note -RG
            // Manually added return models for stored procedures
            //=================================================================
            // Add model entity for the stored Procedure
            //=================================================================
            modelBuilder.Entity<UspGetDatabaseStatisticsReturnModel>().HasNoKey();
            modelBuilder.Entity<UspGetListOfTitlesByCategoryReturnModel>().HasNoKey();

            OnModelCreatingPartial(modelBuilder);
        }

        #endregion Protected Methods

        #region Private Methods

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #endregion Private Methods
    } // END of public partial class DVDStoreDBContext : DbContext
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