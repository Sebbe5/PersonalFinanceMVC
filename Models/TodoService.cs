using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models.Entities;
using PersonalFinanceMVC.Views.Budget;
using PersonalFinanceMVC.Views.Todo;

namespace PersonalFinanceMVC.Models
{
    public class TodoService
    {
        UserManager<ApplicationUser> userManager;
        readonly string userId;

        private readonly ApplicationContext context;
        public TodoService(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor
            )
        {
            this.context = context;
            this.userManager = userManager;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }

        internal TodoListVM CreateTodoListVM()
        {
            var todoItems = context.Todos
                .Where(t => t.ApplicationUserId == userId)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                })
                .ToList();

            

            var userPrefOrder = userManager.Users.FirstOrDefault(u => u.Id == userId).TodoSortingOrder;
            switch (userPrefOrder)
            {
                case TodoSortOrder.AscendingName:
                    todoItems = todoItems.OrderBy(t => t.Name).ToList();
                    break;
                case TodoSortOrder.DescendingName:
                    todoItems = todoItems.OrderByDescending(t => t.Name).ToList();
                    break;
                case TodoSortOrder.AscendingDate:
                    todoItems = todoItems.OrderBy(t => t.Deadline).ToList();
                    break;
                case TodoSortOrder.DescendingDate:
                    todoItems = todoItems.OrderByDescending(t => t.Deadline).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userPrefOrder), userPrefOrder, null);
            }

            return new TodoListVM
            {
                TodoItems = todoItems
            };
        }

        internal string AddTodo(TodoListVM vm)
        {
            var nameOfTodos = new HashSet<string>(context.Todos.Where(t => t.ApplicationUserId == userId)
                                                                .Select(t => t.Name), StringComparer.OrdinalIgnoreCase);

            var validStrings = new string[] { "Work", "Personal", "Other" };

            if (nameOfTodos.Contains(vm.NewTodoItem))
            {
                return "The todo already exists";
            }

            context.Todos.Add(new Todo
            {
                ApplicationUserId = userId,
                Name = vm.NewTodoItem,
                Deadline = vm.NewDeadline,
                Category = validStrings.Contains(vm.NewCategory) ? vm.NewCategory : string.Empty,
            });

            context.SaveChanges();

            return null;
        }

        internal void DeleteTodo(int id)
        {
            context.Todos.Remove(context.Todos.FirstOrDefault(t => t.Id == id));
            context.SaveChanges();
        }

        internal void UserSortSetting(TodoSortOrder sortPreference)
        {
            userManager.Users.FirstOrDefault(u => u.Id == userId).TodoSortingOrder = sortPreference;
            context.SaveChanges();
        }

    }
}
