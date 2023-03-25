using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Budget> Budgets { get; set; }
    }
}
