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

        [Display(Name = "Do Today")]
        public bool ForToday { get; set; }

        [Display(Name = "Use Deadline")]
        public bool ForDeadline { get; set; }
        public List<TodoItemVM> Todos { get; set; } = new List<TodoItemVM>();
        public List<TodoItemVM> InProgress { get; set; } = new List<TodoItemVM>();
        public List<TodoItemVM> Done { get; set; } = new List<TodoItemVM>();
        public List<TodoItemVM> TodosToday { get; set; } = new List<TodoItemVM>();
        public List<TodoItemVM> InProgressToday { get; set; } = new List<TodoItemVM>();
        public List<TodoItemVM> DoneToday { get; set; } = new List<TodoItemVM>();
        public class TodoItemVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Deadline{ get; set; }
            public string Category { get; set; }
            public double DaysToDeadline { get; set; }
            public int Status { get; set; }
            public bool ShowDeadline { get; set; }
        }
    }
}
