using RESTBottle.Models;
using RESTBottle.repostory;

namespace RepositoryTests
{
    public class UnitTest1
    {
        [Fact]
        public void AddTest()
        {
            BottlesRepositoryList bRepo = new BottlesRepositoryList();
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

        public void TestConstructor()
        {
            BottlesRepositoryList bottleRepository = new BottlesRepositoryList(includeTestData: true);

            IEnumerable<Bottle> bottle = bottleRepository.GetBottles();

            Assert.NotNull(bottle);
            Assert.Equal(3, bottle.Count());
        }

        [Fact]
        public void TestConstructorNoData()
        {
            BottlesRepositoryList bottlesRepository = new BottlesRepositoryList(includeTestData: false);

            var bottles = bottlesRepository.GetBottles();

            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }

        [Fact]
        public void TestConstructorDefaultValue()
        {
            BottlesRepositoryList bottlesRepo = new BottlesRepositoryList();

            var bottles = bottlesRepo.GetBottles();

            Assert.NotNull(bottles);
            Assert.Empty(bottles);

        }

        [Fact]
        public void TestGetNoParameter()
        {
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);

            IEnumerable<Bottle> bottles = repo.GetBottles();

            Assert.NotNull(bottles);
            Assert.Equal(3, bottles.Count());

        }

        [Fact]
        public void TestGetNameStartsWith()
        {
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);

            IEnumerable<Bottle> bottles = repo.GetBottles("Bo");

            Assert.Equal(1, bottles.Count());
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
        }

        [Fact]
        public void TestGetNameStartsWithNoMatch()
        {
            //Arrange
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);
            //Act
            IEnumerable<Bottle> bottles = repo.GetBottles("XXX");
            //Assert
            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }

        [Fact]
        public void TestGetNameStartsWithNull()
        {
            //Arrange
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);
            //Act
            IEnumerable<Bottle> bottles = repo.GetBottles(null);
            //Assert
            Assert.Equal(3, bottles.Count());
            Assert.NotNull(bottles);
        }

        [Fact]
        public void TestGetNullName()
        {
            //Arrange
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);

            //Act
            repo.AddBottle(new Bottle() { Name = null, Volume = 100 });
            var bottles = repo.GetBottles("B");

            //arrange
            Assert.Equal(1, bottles.Count());
            Assert.NotNull(bottles);
            Assert.DoesNotContain(bottles, b => b.Name == null);
            Assert.NotEmpty(bottles);
        }
        [Fact]
        public void TestGetMinimumVolume()
        {
            //Arrange
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);
            //Act
            IEnumerable<Bottle> bottles = repo.GetBottles(minimumVolume: 600);
            //Assert
            Assert.Equal(1, bottles.Count());
            Assert.NotNull(bottles);
            Assert.NotEmpty(bottles);
        }
        [Fact]
        public void TestGetMinimumVolumeNoMatch()
        {
            //Arrange
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);
            //Act
            IEnumerable<Bottle> bottles = repo.GetBottles(minimumVolume: 1000);
            //Assert
            Assert.Empty(bottles);
            Assert.NotNull(bottles);
        }
        [Fact]
        public void TestGetMinimumVolumeNull()
        {
            //Arrange
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);
            //Act
            IEnumerable<Bottle> bottles = repo.GetBottles(minimumVolume: null);
            //Assert
            Assert.Equal(3, bottles.Count());
            Assert.NotNull(bottles);


        }
        [Fact]
        public void TestGetSortOrderNameAsc()
        {
            //Arrange
            BottlesRepositoryList repo = new BottlesRepositoryList(includeTestData: true);
            //Act
            IEnumerable<Bottle> bottles = repo.GetBottles(sortOrder: "nameAsc");
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
