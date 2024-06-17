namespace CouncilsManagmentSystem.Models
{
    public class Collage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Department> Department { get; set; }
    }

}
