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
            var todos = GetUserTodos();

            var todoItems = GetFilteredTodos(todos, t => t.Status == Status.ToDo && !t.IsToday);
            var todoItemsToday = GetFilteredTodos(todos, t => t.Status == Status.ToDo & t.IsToday);
            var inProgressItems = GetFilteredTodos(todos, t => t.Status == Status.InProgress & !t.IsToday);
            var inProgressItemsToday = GetFilteredTodos(todos, t => t.Status == Status.InProgress & t.IsToday);
            var doneItems = GetFilteredTodos(todos, t => t.Status == Status.Done & !t.IsToday & t.DaysInDone < 3);
            var doneItemsToday = GetFilteredTodos(todos, t => t.Status == Status.Done & t.IsToday);

            UpdateDaysInDone(doneItems);

            var sortedTodoItems = SortTodoItems(todoItems);

            return new TodoListVM
            {
                Todos = sortedTodoItems,
                InProgress = inProgressItems,
                Done = doneItems,
                TodosToday = todoItemsToday,
                InProgressToday = inProgressItemsToday,
                DoneToday = doneItemsToday,
            };
        }

        private List<TodoListVM.TodoItemVM> SortTodoItems(List<TodoListVM.TodoItemVM> todoItems)
        {
            var userPrefOrder = userManager.Users.FirstOrDefault(u => u.Id == userId).TodoSortingOrder;
            switch (userPrefOrder)
            {
                case TodoSortOrder.AscendingName:
                    return todoItems.OrderBy(t => t.Name).ToList();
                case TodoSortOrder.DescendingName:
                    return todoItems.OrderByDescending(t => t.Name).ToList();
                case TodoSortOrder.AscendingDate:
                    return todoItems.OrderBy(t => t.Deadline).ToList();
                case TodoSortOrder.DescendingDate:
                    return todoItems.OrderByDescending(t => t.Deadline).ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(userPrefOrder), userPrefOrder, null);
            }
        }

        private void UpdateDaysInDone(List<TodoListVM.TodoItemVM> doneItems)
        {
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
        }

        private static List<TodoListVM.TodoItemVM> GetFilteredTodos(IQueryable<Todo> todos, Func<Todo, bool> filter)
        {
            return todos
                .Where(filter)
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
        }

        private IQueryable<Todo> GetUserTodos()
        {
            return context.Todos
                            .Where(t => t.ApplicationUserId == userId);
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
                    ForDeadline = b.NeedDeadline,
                    Description = b.Description
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
            todoToEdit.Description = vm.Description;

            UpdateStatusAndCheckWhenDone(vm.Status, todoToEdit);
            context.SaveChanges();
        }

        internal void EditStatus(int id, string status)
        {
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);
            UpdateStatusAndCheckWhenDone(status, todoToEdit);
            context.SaveChanges();
        }

        private static void UpdateStatusAndCheckWhenDone(string status, Todo todoToEdit)
        {
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
        }

        internal void EditIsToday(int id, bool isToday)
        {
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            todoToEdit.IsToday = isToday;

            context.SaveChanges();
        }
    }
}
