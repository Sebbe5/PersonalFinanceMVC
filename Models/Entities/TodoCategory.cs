namespace PersonalFinanceMVC.Models.Entities
{
    public class TodoCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Todo> Todos { get; set; } = new List<Todo>();
        public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
    }
}
