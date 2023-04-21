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
                })
                .ToList();

            var userPrefOrder = userManager.Users.FirstOrDefault(u => u.Id == userId).SortingOrder;
            switch (userPrefOrder)
            {
                case SortOrder.AscendingName:
                    todoItems = todoItems.OrderBy(t => t.Name).ToList();
                    break;
                case SortOrder.DescendingName:
                    todoItems = todoItems.OrderByDescending(t => t.Name).ToList();
                    break;
                case SortOrder.AscendingDate:
                    todoItems = todoItems.OrderBy(t => t.Deadline).ToList();
                    break;
                case SortOrder.DescendingDate:
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
            var existingTodo = context.Todos.Where(t => t.ApplicationUserId == userId).FirstOrDefault(t => t.Name == vm.NewTodoItem);

            if (existingTodo != null)
            {
                return "The todo already exists";
            }

            context.Todos.Add(new Todo
            {
                ApplicationUserId = userId,
                Name = vm.NewTodoItem,
                Deadline = vm.NewDeadline
            });

            context.SaveChanges();

            return null;
        }

        internal void DeleteTodo(int id)
        {
            context.Todos.Remove(context.Todos.FirstOrDefault(t => t.Id == id));
            context.SaveChanges();
        }

        internal void UserSortSetting(SortOrder sortPreference)
        {
            userManager.Users.FirstOrDefault(u => u.Id == userId).SortingOrder = sortPreference;
            context.SaveChanges();
        }

    }
}
