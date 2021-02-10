namespace Academy.Domain
{
    public class Section
    {
        public Section(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; }
    }
}