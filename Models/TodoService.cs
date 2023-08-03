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
            // Fetch the users todos
            var todos = GetUserTodos();

            // Set a list of todo items depending on status
            var todoItems = GetFilteredTodos(todos, t => t.Status == Status.ToDo && !t.IsToday);
            var todoItemsToday = GetFilteredTodos(todos, t => t.Status == Status.ToDo & t.IsToday);
            var inProgressItems = GetFilteredTodos(todos, t => t.Status == Status.InProgress & !t.IsToday);
            var inProgressItemsToday = GetFilteredTodos(todos, t => t.Status == Status.InProgress & t.IsToday);
            var doneItems = GetFilteredTodos(todos, t => t.Status == Status.Done & !t.IsToday & t.DaysInDone < 3);
            var doneItemsToday = GetFilteredTodos(todos, t => t.Status == Status.Done & t.IsToday);

            // Update amount of days tickets have been in done
            UpdateDaysInDone(doneItems);

            // Sort the todo items
            var sortedTodoItems = SortTodoItems(todoItems);

            // Return a new instance of a TodoListVm
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
        internal string AddTodo(TodoListVM vm)
        {
            // Set list of names of all todos
            var nameOfTodos = new HashSet<string>(context.Todos.Where(t => t.ApplicationUserId == userId)
                .Select(t => t.Name), StringComparer.OrdinalIgnoreCase);

            // Not in use anymore?
            //var validStrings = new string[] { "Work", "Personal", "Other" };

            // Check if the new TodoItem already exists
            if (nameOfTodos.Contains(vm.NewTodoItem))
            {
                return "The todo already exists";
            }
            
            // Add the new todos to the database
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
            // Remove todo with matching id
            context.Todos.Remove(context.Todos.FirstOrDefault(t => t.Id == id));
            context.SaveChanges();
        }
        internal void UserSortSetting(TodoSortOrder sortPreference)
        {
            // Fetch the users prefereable sorting order (Ís this active?)
            userManager.Users.FirstOrDefault(u => u.Id == userId).TodoSortingOrder = sortPreference;
            context.SaveChanges();
        }
        internal EditVM CreateEditVM(int id)
        {
            // Return a new instance of an EditVM
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
            // Fetch the todo with matching Id
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            // Set properties
            todoToEdit.Name = vm.Name;
            todoToEdit.Deadline = vm.Deadline;
            todoToEdit.Category = vm.Category;
            todoToEdit.NeedDeadline = vm.ForDeadline;
            todoToEdit.Description = vm.Description;

            // Update status
            UpdateStatusAndCheckWhenDone(vm.Status, todoToEdit);
            context.SaveChanges();
        }
        internal void EditStatus(int id, string status)
        {
            // Fetch todo with id
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            // Update status
            UpdateStatusAndCheckWhenDone(status, todoToEdit);
            context.SaveChanges();
        }
        private static void UpdateStatusAndCheckWhenDone(string status, Todo todoToEdit)
        {
            Status newStatus;

            // Check if the new status if of enum status
            Enum.TryParse(status, true, out newStatus);

            // If the new status is done, set DateDone to the current date
            if (newStatus == Status.Done && todoToEdit.Status != Status.Done)
                todoToEdit.DateDone = DateTime.Now;
            // Else set DateDone to null
            else if (newStatus != Status.Done)
                todoToEdit.DateDone = null;

            // Set the new status
            todoToEdit.Status = newStatus;

            // If the Todo's date done has a value update DaysInDone
            if (todoToEdit.DateDone.HasValue)
            {
                todoToEdit.DaysInDone = (DateTime.Now - todoToEdit.DateDone.Value).Days;
            }
            // Else set days in done to 0
            else
            {
                todoToEdit.DaysInDone = 0;
            }
        }
        internal void EditIsToday(int id, bool isToday)
        {
            // Fetch todo with matching id
            var todoToEdit = context.Todos.SingleOrDefault(t => t.Id == id);

            // Set the bool isToday to true or false depending on input
            todoToEdit.IsToday = isToday;

            context.SaveChanges();
        }
        private List<TodoListVM.TodoItemVM> SortTodoItems(List<TodoListVM.TodoItemVM> todoItems)
        {
            // Fetch the users preferable sorting order (is this used?)
            var userPrefOrder = userManager.Users.FirstOrDefault(u => u.Id == userId).TodoSortingOrder;

            // Set the order of todos depending on userPrefOrder
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
                // Fetch todo with matching id
                var todo = context.Todos.FirstOrDefault(t => t.Id == doneItem.Id);
                if (todo != null)
                {
                    // TODO: Continue commenting here
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
        private IQueryable<Todo> GetUserTodos() => context.Todos.Where(t => t.ApplicationUserId == userId);
    }
}
