namespace CouncilsManagmentSystem.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int NumberOfSeats { get; set; }
        public List<Councils> Councils { get; set; }
    }
}
