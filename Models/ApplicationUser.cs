using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceMVC.Models
{
    // TODO: COntinue commenting here
    public class ApplicationUser : IdentityUser
    {
        public List<Budget> Budgets { get; set; }
        public List<Todo> Todos { get; set; }
        public TodoSortOrder TodoSortingOrder { get; set; } = new TodoSortOrder();
    }

    public enum TodoSortOrder
    {
        AscendingName,
        DescendingName,
        AscendingDate,
        DescendingDate,
    }
}
