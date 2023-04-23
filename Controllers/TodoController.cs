using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Todo;

namespace PersonalFinanceMVC.Controllers

    // TODO: Add the ability to edit an existing todo
    // TODO: Add bool to the VM depending on how close to deadline a task it. Then change color of the background or something.
    // TODO: Add a search function
    // TODO: Add comments
    // TODO: Add tags
    // TODO: Add subtassks
    // TODO: Make it like a kanban board?
    // TODO: Add a computer reminder (Probably not possible until launched)
{
    public class TodoController : Controller
    {
        TodoService todoService;
        public TodoController(TodoService todoService)
        {
            this.todoService = todoService;
        }

        [HttpGet("/TodoList")]
        public IActionResult TodoList()
        {
            TodoListVM vm = todoService.CreateTodoListVM();
            return View(vm);
        }

        [HttpPost("/TodoList")]
        public IActionResult TodoList(TodoListVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(todoService.CreateTodoListVM());
            }

            var errorMessage = todoService.AddTodo(vm);
            if (errorMessage != null)
            {
                // Show error
                TempData["Message"] = errorMessage;
                return View(todoService.CreateTodoListVM());
            }


            return RedirectToAction(nameof(TodoList));
        }

        [HttpPost("/deleteTodo")]
        public IActionResult Delete(int id)
        {
            todoService.DeleteTodo(id);
            return RedirectToAction(nameof(TodoList));
        }

        [HttpPost("/sortTodo")]
        public IActionResult Sort(string sortOrder)
        {
            TodoSortOrder sortPreference;
            Enum.TryParse(sortOrder, out sortPreference);
            todoService.UserSortSetting(sortPreference);
            return RedirectToAction(nameof(TodoList));
        }


    }
}
