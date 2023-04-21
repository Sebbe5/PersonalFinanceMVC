using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Budget> Budgets { get; set; }
        public List<Todo> Todos { get; set; }
        public SortOrder SortingOrder { get; set; } = new SortOrder();

    }

    public enum SortOrder
    {
        AscendingName,
        DescendingName,
        AscendingDate,
        DescendingDate,
    }
}
