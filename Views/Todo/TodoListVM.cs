using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceMVC.Views.Todo
{
    public class TodoListVM
    {
        
        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "New Todo Item", Prompt ="New Todo Item")]
        public string NewTodoItem { get; set; }

        [Display(Name = "Deadline")]
        [DefaultValue(typeof(DateTime), "{0:yyyy-MM-dd HH:mm}")]
        public DateTime NewDeadline { get; set; } = DateTime.Now.AddHours(1);

        [Display(Name = "Category", Prompt = "Category")]
        public string NewCategory { get; set; }
        public List<TodoItemVM> TodoItems { get; set; } = new List<TodoItemVM>();
        public class TodoItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Deadline{ get; set; }
            public string Category { get; set; }
        }
    }
}
