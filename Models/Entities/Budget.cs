using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceMVC.Models.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Expense> Expenses { get; set; }
        public string ApplicationUserId { get; set; }

        ApplicationUser ApplicationUser { get; set; }

        
        
    }
}
