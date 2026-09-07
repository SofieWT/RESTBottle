using RESTBottle.Models;

namespace RESTBottle.repostory
{
    public interface IBottlesRepository
    {
        Bottle AddBottle(Bottle b);
        Bottle DeleteById(int id);
        List<Bottle> Get();
        IEnumerable<Bottle> GetBottles(string? nameStartsWith = null, double? minimumVolume = null, string? sortOrder = null);
        Bottle? GetById(int id);
        Bottle? Update(int id, Bottle data);
    }
}