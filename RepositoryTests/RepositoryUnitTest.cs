using Microsoft.EntityFrameworkCore;
using RESTBottle.Interfaces;
using RESTBottle.Models;
using RESTBottle.repostory;
using RESTBottle.Service;

namespace RepositoryTests
{
    public class RepoditoryUnitTest
    {

        private bool useDatabase = true; // Set to true to use the database repository, false to use the list repository
        private IBottlesRepository bRepo;

        public RepoditoryUnitTest()
        {
            //you can switch between the two repository implementations by changing the value of useDatabase.
            if (useDatabase) {
                var optionsBuilder = new DbContextOptionsBuilder<BottlesDBContext>();
                // https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets
                optionsBuilder.UseSqlServer(Secret.GetConnectionString);
                // connection string structure
                //   "Data Source=mssql7.unoeuro.com;Initial Catalog=FROM simply.com;Persist Security Info=True;User ID=FROM simply.com;Password=DB PASSWORD FROM simply.com;TrustServerCertificate=True"
                BottlesDBContext _dbContext = new(optionsBuilder.Options);
                // clean database table: remove all rows
                _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Bottles");
                bRepo = new BottleRepositoryDatabase(_dbContext);
            }
            else 
            {
                //use the list repository with test data
                bRepo = new BottlesRepositoryList();
            }
        }

        [Fact]
        public void AddTest()
        {            
            Bottle b1 = new Bottle()
            {
                Name = "Test",
                Volume = 500
            };
            Bottle addedBottle = bRepo.AddBottle(b1);

            Assert.NotNull(addedBottle.Id);
            Assert.Equal(1, addedBottle.Id);
            //Assert.Equal(1,bRepo.Get().Count());
            //Assert.Single(bRepo.Get());
            Assert.Equal("Test", b1.Name);
            Assert.Equal(500, b1.Volume);
        }

        [Fact]
        public void DeleteTest()
        {
            Bottle b1 = new Bottle()
            {
                Name = "Test",
                Volume = 500
            };
            Bottle addedBottle = bRepo.AddBottle(b1);
            Bottle deletedBottle = bRepo.DeleteById(addedBottle.Id);

            Assert.NotNull(deletedBottle);
            Assert.Equal(addedBottle.Id, deletedBottle.Id);
            Assert.Equal(0, bRepo.GetBottles().Count());
        }

        [Fact]
        public void DeleteNonExistingBottleTest()
        {
            Bottle deletedBottle = bRepo.DeleteById(999); // Assuming 999 is a non-existing ID

            Assert.Null(deletedBottle);
            Assert.Equal(0, bRepo.GetBottles().Count());
            Assert.Empty(bRepo.GetBottles());
            Assert.NotNull(bRepo.GetBottles());
        }

        [Fact]
        public void UpdateTest()
        {
            Bottle b1 = new Bottle()
            {
                Name = "Test",
                Volume = 500
            };
            Bottle addedBottle = bRepo.AddBottle(b1);
            Bottle updatedData = new Bottle()
            {
                Name = "Updated Test",
                Volume = 600
            };
            Bottle updatedBottle = bRepo.Update(addedBottle.Id, updatedData);
            Assert.NotNull(updatedBottle);
            Assert.Equal(addedBottle.Id, updatedBottle.Id);
            Assert.Equal("Updated Test", updatedBottle.Name);
            Assert.Equal(600, updatedBottle.Volume);
        }

        [Fact]
        public void UpdateNonExistingBottleTest()
        {
            Bottle updatedData = new Bottle()
            {
                Name = "Updated Test",
                Volume = 600
            };

            Bottle updatedBottle = bRepo.Update(999, updatedData); // Assuming 999 is a non-existing ID

            Assert.Null(updatedBottle);
            Assert.Equal(0, bRepo.GetBottles().Count());
            Assert.Empty(bRepo.GetBottles());
            Assert.NotNull(bRepo.GetBottles());
            Assert.DoesNotContain(bRepo.GetBottles(), b => b.Name == "Updated Test");
        }

        [Fact]
        public void GetByIdTest()
        {
            Bottle b1 = new Bottle()
            {
                Name = "Test",
                Volume = 500
            };

            //Act
            Bottle addedBottle = bRepo.AddBottle(b1);
            Bottle retrievedBottle = bRepo.GetById(addedBottle.Id);
            //Assert
            Assert.NotNull(retrievedBottle);
            Assert.Equal(addedBottle.Id, retrievedBottle.Id);
            Assert.Equal("Test", retrievedBottle.Name);
            Assert.Equal(500, retrievedBottle.Volume);
        }



        //[Fact]
        public void TestConstructor()
        {
            IEnumerable<Bottle> bottle = bRepo.GetBottles();

            Assert.NotNull(bottle);
            Assert.Equal(3, bottle.Count());
        }

        [Fact]
        public void TestConstructorNoData()
        {

            var bottles = bRepo.GetBottles();

            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }

        [Fact]
        public void TestConstructorDefaultValue()
        {

            var bottles = bRepo.GetBottles();

            Assert.NotNull(bottles);
            Assert.Empty(bottles);

        }

        //[Fact]
        public void TestGetNoParameter()
        {

            IEnumerable<Bottle> bottles = bRepo.GetBottles();

            Assert.NotNull(bottles);
            Assert.Equal(3, bottles.Count());

        }

        //[Fact]
        public void TestGetNameStartsWith()
        {

            IEnumerable<Bottle> bottles = bRepo.GetBottles("Bo");

            Assert.Equal(1, bottles.Count());
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
        }

        [Fact]
        public void TestGetNameStartsWithNoMatch()
        {

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles("XXX");
            //Assert
            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }

        //[Fact]
        public void TestGetNameStartsWithNull()
        {
            //Arrange

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(null);
            //Assert
            Assert.Equal(3, bottles.Count());
            Assert.NotNull(bottles);
        }

        //[Fact]
        public void TestGetNullName()
        {

            //Act
            bRepo.AddBottle(new Bottle() { Name = null, Volume = 100 });
            var bottles = bRepo.GetBottles("B");

            //arrange
            Assert.Equal(1, bottles.Count());
            Assert.NotNull(bottles);
            Assert.DoesNotContain(bottles, b => b.Name == null);
            Assert.NotEmpty(bottles);
        }
        //[Fact]
        public void TestGetMinimumVolume()
        {
            //Arrange

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: 600);
            //Assert
            Assert.Equal(1, bottles.Count());
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
        }
        [Fact]
        public void TestGetMinimumVolumeNoMatch()
        {
            //Arrange

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: 1000);
            //Assert
            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }
        //[Fact]
        public void TestGetMinimumVolumeNull()
        {
            //Arrange

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: null);
            //Assert
            Assert.Equal(3, bottles.Count());
            Assert.NotNull(bottles);


        }
        //[Fact]
        public void TestGetSortOrderNameAsc()
        {
            //Arrange

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(sortOrder: "nameAsc");
            //Assert
            Assert.Equal(3, bottles.Count());
            Assert.NotNull(bottles);
            Assert.Equal("Bottle", bottles.First().Name);
        }
        //[Fact]
        //public void TestGetSortOrderNameDesc()
        //{
        //    //Arrange
        //    BottlesRepository repo = new BottlesRepository(includeTestData: true);
        //    //Act
        //    IEnumerable<Bottle> bottles = repo.GetBottles(sortOrder: "nameDesc");
        //    //Assert
        //    Assert.Equal(3, bottles.Count());
        //    Assert.NotNull(bottles);
        //    Assert.Equal("Third Bottle", bottles.First().Name);
        //}

    }
}
