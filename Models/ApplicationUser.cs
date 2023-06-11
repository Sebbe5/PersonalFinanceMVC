using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        // The Budgets property represents a collection of budgets associated with the user.
        public List<Budget> Budgets { get; set; }

        // The Todos property represents a collection of to-do items associated with the user.
        public List<Todo> Todos { get; set; }

        // The TodoSortingOrder property represents the sorting order for the to-do items.
        // It is of type TodoSortOrder, which is an enumeration defined below.
        // The default sorting order is set to AscendingName.
        public TodoSortOrder TodoSortingOrder { get; set; } = new TodoSortOrder();
    }

    // The TodoSortOrder enumeration represents the possible sorting orders for to-do items.
    // It defines four options: AscendingName, DescendingName, AscendingDate, and DescendingDate.
    public enum TodoSortOrder
    {
        AscendingName,
        DescendingName,
        AscendingDate,
        DescendingDate,
    }
}
