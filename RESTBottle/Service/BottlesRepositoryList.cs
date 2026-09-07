using RESTBottle.Models;

namespace RESTBottle.repostory
{
    public class BottlesRepositoryList : IBottlesRepository
    {

        private List<Bottle> _bottles = new List<Bottle>();

        private int _nextId = 1;

        public BottlesRepositoryList(bool includeTestData = false)
        {
            if (includeTestData)
            {
                AddBottle(new Bottle { Name = "Bottle", Volume = 500 });
                AddBottle(new Bottle { Name = "New Bottle", Volume = 500 });
                AddBottle(new Bottle { Name = "Third Bottle", Volume = 750 });
            }
        }
        public List<Bottle> Get() //Her kan der stadig laves om i selve objekterne i listen, derfor er IEnumerable bedre på den måde.
        {
            return _bottles.ToList();
        }

        public IEnumerable<Bottle> GetBottles(string? nameStartsWith = null, double? minimumVolume = null,
                                              string? sortOrder = null) //Det kan udvides med om der skal sorteres ascending eller descending, men det er ikke lige nu.
        {
            IEnumerable<Bottle> result = _bottles.AsReadOnly();
            if (minimumVolume != null) //Der skal soretes efter at der
            {
                result = result.Where(b => b.Volume > minimumVolume);
            }
            if (nameStartsWith != null)
            {
                result = result.Where(b => b.Name != null && b.Name.StartsWith(nameStartsWith));
            }
            if (sortOrder != null)
            {
                switch (sortOrder.ToLower())
                {
                    case "name":
                    case "nameAsc":
                        result = result.OrderBy(b => b.Name);
                        break;
                    case "nameDesc":
                        result = result.OrderByDescending(b => b.Name);
                        break;
                    case "volume":
                    case "volumeAsc":
                        result = result.OrderBy(b => b.Volume);
                        break;
                    case "volumeDesc":
                        result = result.OrderByDescending(b => b.Volume);
                        break;
                    default:
                        break;
                }

            }
            return result;

        }

        public Bottle AddBottle(Bottle b)
        {
            b.Id = _nextId++;
            _bottles.Add(b);
            return b;
        }

        public Bottle? GetById(int id)
        {
            return _bottles.FirstOrDefault(b => b.Id == id);
        }

        public Bottle DeleteById(int id)
        {
            var bottle = GetById(id);
            if (bottle != null)
            {
                _bottles.Remove(bottle);
            }
            return bottle;
        }

        public Bottle? Update(int id, Bottle data)
        {
            Bottle? b = GetById(id);
            if (b != null)
            {
                b.Volume = data.Volume;
                b.Name = data.Name;
            }
            return b;
        }
    }
}
