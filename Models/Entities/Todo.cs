namespace PersonalFinanceMVC.Models.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Category { get; set; }
        public Status Status { get; set; } = new Status();
        public bool IsToday { get; set; } = false;
        public bool NeedDeadline { get; set; } = false;
        public int DaysInDone { get; set; }
        public DateTime? DateDone { get; set; }
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
