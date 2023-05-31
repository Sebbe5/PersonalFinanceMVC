using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Todo
{
    public class EditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; } = DateTime.Now.AddHours(2);
        public string Category { get; set; }
        public string Status { get; set; }

        [Display(Name = "Use Deadline")]
        public bool ForDeadline { get; set; }
        public string Description { get; set; }
    }
}
