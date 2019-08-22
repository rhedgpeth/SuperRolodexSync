namespace SuperRolodex.Core.Models
{
    public class Hero
    {
        public string Type => "hero";
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public byte[] ImageData { get; set; }
    }
}
