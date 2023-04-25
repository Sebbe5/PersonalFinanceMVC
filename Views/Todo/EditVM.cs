using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Todo
{
    public class EditVM
    {
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Category { get; set; }
    }
}
