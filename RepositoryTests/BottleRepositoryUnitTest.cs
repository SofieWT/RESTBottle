using Microsoft.EntityFrameworkCore;
using RESTBottle.Interfaces;
using RESTBottle.Models;
using RESTBottle.repostory;
using RESTBottle.Service;

namespace RepositoryTests
{
    public class BottleRepositoryUnitTest
    {

        private bool useDatabase = true; //Set to true to use the database repository, false to use the list repository
        private bool useInMemoryDatabase = true; //Set to true to use an in-memory database for testing, false to use a real database
        private IBottlesRepository bRepo;

        private Bottle b1 = new Bottle()
        {
            Name = "Test bottle",
            Volume = 500
        };
        private Bottle b2 = new Bottle()
        {
            Name = "Bottle",
            Volume = 750
        };

        public RepoditoryUnitTest()
        {
            //you can switch between the two repository implementations by changing the value of useDatabase.
            if (useDatabase)
            {

                if (useInMemoryDatabase)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<BottlesDBContext>();
                    optionsBuilder.UseInMemoryDatabase("TestDatabase");
                    BottlesDBContext _dbContext = new(optionsBuilder.Options);
                    _dbContext.Database.EnsureDeleted();
                    _dbContext.Database.EnsureCreated();
                    bRepo = new BottleRepositoryDatabase(new BottlesDBContext(optionsBuilder.Options));

                }
                else
                {
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

        //[Fact]
        public void TestConstructorNoData()
        {

            var bottles = bRepo.GetBottles();

            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }

        //[Fact]
        public void TestConstructorDefaultValue()
        {

            var bottles = bRepo.GetBottles();

            Assert.NotNull(bottles);
            Assert.Empty(bottles);

        }

        [Fact]
        public void TestGetNoParameter()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles();

            //assert
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
            Assert.Equal(2, bottles.Count());

        }

        [Fact]
        public void TestGetNameStartsWith()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles("Bo");
            //Assert
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
            Assert.Equal(1, bottles.Count());
            Assert.Equal("Bottle", bottles.First().Name);
        }

        [Fact]
        public void TestGetNameStartsWithNoMatch()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles("XXX");
            //Assert
            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }

        [Fact]
        public void TestGetNameStartsWithNull()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(null);
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
        }

        [Fact]
        public void TestGetNameStartsWithHasNullName()
        {
            //Arrange
            Bottle b3 = new Bottle()
            {
                Name = null,
                Volume = 1000
            };
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            bRepo.AddBottle(b3);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles("Bo");
            //Assert
            Assert.Equal(1, bottles.Count());
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
        }

        [Fact]
        public void TestGetMinimumVolume()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: 500);
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
        }
        [Fact]
        public void TestGetMinimumVolumeNoMatch()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: 1000);
            //Assert
            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }
        [Fact]
        public void TestGetMinimumVolumeNull()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: null);
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);


        }

        [Fact]
        public void TestGetSortOrderNameAscNoNameStartsWith()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);

            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(sortOrder: "name_asc");
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);
            Assert.Equal("Bottle", bottles.First().Name);
            Assert.Equal("Test bottle", bottles.Last().Name);
            Assert.Equal(750, bottles.First().Volume);
            Assert.Equal(500, bottles.Last().Volume);
        }

        [Fact]
        public void TestGetSortOrderNameAscWithNameStartsWith()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(nameStartsWith: "Bo", sortOrder: "name_asc");
            //Assert
            Assert.Single(bottles);
            Assert.NotNull(bottles);
            Assert.Equal("Bottle", bottles.First().Name);
            Assert.Equal(750, bottles.First().Volume);
        }

        [Fact]
        public void TestGetSortOrderVolumeAscNoVolumeMin()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(sortOrder: "volume_asc");
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);
            Assert.Equal("Test bottle", bottles.First().Name);
            Assert.Equal("Bottle", bottles.Last().Name);
            Assert.Equal(500, bottles.First().Volume);
            Assert.Equal(750, bottles.Last().Volume);
        }
        [Fact]
        public void TestGetSortOrderVolumeAscWithVolumeMin()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: 500, sortOrder: "volume_asc");
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);
            Assert.Equal("Test bottle", bottles.First().Name);
            Assert.Equal("Bottle", bottles.Last().Name);
            Assert.Equal(500, bottles.First().Volume);
            Assert.Equal(750, bottles.Last().Volume);
        }

        [Fact]
        public void TestGetSortOrderNameAscWithVolumeMin()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(minimumVolume: 500, sortOrder: "name_asc");
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);
            Assert.Equal("Bottle", bottles.First().Name);
            Assert.Equal("Test bottle", bottles.Last().Name);
            Assert.Equal(750, bottles.First().Volume);
            Assert.Equal(500, bottles.Last().Volume);
        }
        [Fact]
        public void TestGetSortOrderNameAscWithVolumeMinAndNameStartsWith()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(nameStartsWith: "Bo", minimumVolume: 500, sortOrder: "name_asc");
            //Assert
            Assert.Single(bottles);
            Assert.NotNull(bottles);
            Assert.Equal("Bottle", bottles.First().Name);
            Assert.Equal(750, bottles.First().Volume);
        }
        [Fact]
        public void TestGetSortOrderNotRecognized()
        {
            //Arrange
            bRepo.AddBottle(b1);
            bRepo.AddBottle(b2);
            //Act
            IEnumerable<Bottle> bottles = bRepo.GetBottles(sortOrder: "invalid_sort_order");
            //Assert
            Assert.Equal(2, bottles.Count());
            Assert.NotNull(bottles);
            Assert.Equal("Test bottle", bottles.First().Name);
            Assert.Equal("Bottle", bottles.Last().Name);
        }
        //[Fact]
        
    }
}
