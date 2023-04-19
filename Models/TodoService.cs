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
            return new TodoListVM
            {
                TodoItems = context.Todos.Where(t => t.ApplicationUserId == userId).Select(t => t.Name).ToList()
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
                Name = vm.NewTodoItem
            });

            context.SaveChanges();

            return null;
        }

        internal void DeleteTodo(string name)
        {
            context.Todos.Remove(context.Todos.FirstOrDefault(t => t.Name == name));
            context.SaveChanges();
        }

    }
}
