using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Todo
{
    public class TodoListVM
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "New Todo Item", Prompt ="New Todo Item")]
        public string NewTodoItem { get; set; }

        public List<string> TodoItems { get; set; } = new List<string>();
    }
}
