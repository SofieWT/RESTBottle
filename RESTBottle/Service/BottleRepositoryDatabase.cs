using RESTBottle.Interfaces;
using RESTBottle.Models;

namespace RESTBottle.Service
{
    public class BottleRepositoryDatabase : IBottlesRepository
    {

        private BottlesDBContext _context;
        public BottleRepositoryDatabase(BottlesDBContext context)
        {
            _context = context;
        }
        public Bottle AddBottle(Bottle b)
        {
            var addedBottle = _context.Bottles.Add(b);
            _context.SaveChanges(); // Save changes to the database - denne er MEGET nødvendig for at ændringerne bliver gemt i databasen
            return addedBottle.Entity;
        }

        public Bottle DeleteById(int id)
        {
            var bottleToDelete = GetById(id);
            if (bottleToDelete == null) return null;
            _context.Bottles.Remove(bottleToDelete);
            _context.SaveChanges();
            return bottleToDelete;
        }

        public List<Bottle> Get()
        {
            return _context.Bottles.ToList();
        }

        public IEnumerable<Bottle> GetBottles(string? nameStartsWith = null, double? minimumVolume = null, string? sortOrder = null)
        {
            //TODO: Implement filtering and sorting logic based on the provided parameters.
            return _context.Bottles;
        }

        public Bottle? GetById(int id)
        {
            return _context.Bottles.FirstOrDefault(b => b.Id == id);//Her skal SaveChanges ikke bruges, da vi kun henter data og ikke ændrer noget i databasen.
        }
        
        public Bottle? Update(int id, Bottle data)
        {
            var bottleToUpdate = GetById(id);
            if(bottleToUpdate == null) return null;
            bottleToUpdate.Name = data.Name;
            bottleToUpdate.Volume = data.Volume;
            _context.SaveChanges();
            return bottleToUpdate;
        }
    }
}