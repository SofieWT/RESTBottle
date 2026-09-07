using RESTBottle.Models;
using RESTBottle.repostory;

namespace RESTBottle.Service
{
    public class BottleRepositoryDatabase : IBottlesRepository
    {
        public Bottle AddBottle(Bottle b)
        {
            throw new NotImplementedException();
        }

        public Bottle DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Bottle> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bottle> GetBottles(string? nameStartsWith = null, double? minimumVolume = null, string? sortOrder = null)
        {
            throw new NotImplementedException();
        }

        public Bottle? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Bottle? Update(int id, Bottle data)
        {
            throw new NotImplementedException();
        }
    }
}
