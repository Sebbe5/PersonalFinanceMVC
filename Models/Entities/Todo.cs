﻿namespace PersonalFinanceMVC.Models.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Category { get; set; }
        public string ApplicationUserId { get; set; }
        ApplicationUser ApplicationUser { get; set; }
    }
}
