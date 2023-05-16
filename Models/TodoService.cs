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
                    Status = (int)t.Status,
                    ShowDeadline = t.NeedDeadline
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
                    Status = (int)t.Status,
                    ShowDeadline = t.NeedDeadline

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
                    Status = (int)t.Status,
                    ShowDeadline = t.NeedDeadline

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
                    Status = (int)t.Status,
                    ShowDeadline = t.NeedDeadline

                })
                .ToList();

            var doneItems = todos
                .Where(t => t.Status == Status.Done & !t.IsToday & t.DaysInDone < 3)
                .Select(t => new TodoListVM.TodoItemVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Category = t.Category,
                    DaysToDeadline = (t.Deadline - DateTime.Now).TotalDays,
                    Status = (int)t.Status,
                    ShowDeadline = t.NeedDeadline

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
                    Status = (int)t.Status,
                    ShowDeadline = t.NeedDeadline

                })
                .ToList();

            // Update DaysInDone property for relevant Todos
            foreach (var doneItem in doneItems)
            {
                var todo = context.Todos.FirstOrDefault(t => t.Id == doneItem.Id);
                if (todo != null)
                {
                    todo.DaysInDone = (DateTime.Now - todo.DateDone.Value).Days;
                    context.SaveChanges();
                }
            }


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
                IsToday = vm.ForToday,
                NeedDeadline = vm.ForDeadline
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
                    Id = id,
                    ForDeadline = b.NeedDeadline
                })
                .SingleOrDefault();
        }

        internal void EditTodo(EditVM vm, int id)
        {
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            todoToEdit.Name = vm.Name;
            todoToEdit.Deadline = vm.Deadline;
            todoToEdit.Category = vm.Category;
            todoToEdit.NeedDeadline = vm.ForDeadline;

            // TODO: This code is used in the below method as well

            Status newStatus;
            Enum.TryParse(vm.Status, true,  out newStatus);

            if (newStatus == Status.Done && todoToEdit.Status != Status.Done)
                todoToEdit.DateDone = DateTime.Now;
            else if (newStatus != Status.Done)
                todoToEdit.DateDone = null;

            todoToEdit.Status = newStatus;

            if (todoToEdit.DateDone.HasValue)
            {
                todoToEdit.DaysInDone = (DateTime.Now - todoToEdit.DateDone.Value).Days;
            }
            else
            {
                todoToEdit.DaysInDone = 0;
            }

            context.SaveChanges();
        }

        internal void EditStatus(int id, string status)
        {
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            Status newStatus;
            Enum.TryParse(status, true, out newStatus);

            if (newStatus == Status.Done && todoToEdit.Status != Status.Done)
                todoToEdit.DateDone = DateTime.Now;
            else if (newStatus != Status.Done)
                todoToEdit.DateDone = null;

            todoToEdit.Status = newStatus;

            if (todoToEdit.DateDone.HasValue)
            {
                todoToEdit.DaysInDone = (DateTime.Now - todoToEdit.DateDone.Value).Days;
            }
            else
            {
                todoToEdit.DaysInDone = 0;
            }

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
