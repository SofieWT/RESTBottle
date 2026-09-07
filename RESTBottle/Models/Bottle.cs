namespace RESTBottle.Models
{
    public class Bottle
    {
        public int Id { get; set; }
        public double Volume { get; set; }//Når det er en valuetype er default 0
        public string? Name { get; set; } //Når det er en reference type er denne nulable - der peges på ingen ting

        public Bottle()
        {
            
        }

        public Bottle(int id, double volume, string? name)
        {
            Id = id;
            Volume = volume;
            Name = name;
        }

        public override string ToString()
        {
            return $"Bottle: {Name}, Volume: {Volume}";
        }
    }
}
