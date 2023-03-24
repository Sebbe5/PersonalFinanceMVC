using Microsoft.AspNetCore.Identity;
using PersonalFinanceMVC.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("UserID")]
        public List<Budget> Budgets{ get; set; }
    }
}
