namespace PersonalFinanceMVC.Models.Entities
{

    // TODO: Allow null for deadline
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Category { get; set; }
        public Status Status { get; set; } = new Status();
        public bool IsToday { get; set; } = false;
        public string ApplicationUserId { get; set; }
        ApplicationUser ApplicationUser { get; set; }
    }

    public enum Status
    {
        ToDo,
        InProgress,
        Done
    }
}
