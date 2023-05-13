using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
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
            var todos = context.Todos
                .Where(t => t.ApplicationUserId == userId);

            var todoItems = todos
                .Where(t => t.Status == Status.ToDo & !t.IsToday)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                    Status = (int)t.Status
                })
                .ToList();
            
            var todoItemsToday = todos
                .Where(t => t.Status == Status.ToDo & t.IsToday)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                    Status = (int)t.Status
                })
                .ToList();

            var inProgressItems = todos
                .Where(t => t.Status == Status.InProgress & !t.IsToday)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                    Status = (int)t.Status
                })
                .ToList();

            var inProgressItemsToday = todos
                .Where(t => t.Status == Status.InProgress & t.IsToday)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                    Status = (int)t.Status
                })
                .ToList();

            var doneItems = todos
                .Where(t => t.Status == Status.Done & !t.IsToday)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                    Status = (int)t.Status
                })
                .ToList();

            var doneItemsToday = todos
                .Where(t => t.Status == Status.Done & t.IsToday)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                    Status = (int)t.Status
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
                Todos = todoItems,
                InProgress = inProgressItems,
                Done = doneItems,
                TodosToday = todoItemsToday,
                InProgressToday = inProgressItemsToday,
                DoneToday = doneItemsToday,
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
                Category = vm.NewCategory,
                Status = Status.ToDo,
                IsToday = vm.ForToday
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

        internal EditVM CreateEditVM(int id)
        {
            return context.Todos
                .Where(t => t.Id == id)
                .Select(b => new EditVM
                {
                    Name = b.Name,
                    Deadline = b.Deadline,
                    Category = b.Category,
                    Status = b.Status.ToString(),
                    Id = id
                })
                .SingleOrDefault();
        }

        internal void EditTodo(EditVM vm, int id)
        {
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);


            todoToEdit.Name = vm.Name;
            todoToEdit.Deadline = vm.Deadline;
            todoToEdit.Category = vm.Category;
            Status newStatus;
            // Convert to enum and ignore casing
            Enum.TryParse(vm.Status, true,  out newStatus);
            todoToEdit.Status = newStatus;
            

            context.SaveChanges();
        }

        internal void EditStatus(int id, string status)
        {
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            Status newStatus;
            Enum.TryParse(status, true, out newStatus);
            todoToEdit.Status = newStatus;

            context.SaveChanges();
        }

        internal void EditIsToday(int id, bool isToday)
        {
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            todoToEdit.IsToday = isToday;

            context.SaveChanges();
        }
    }
}
