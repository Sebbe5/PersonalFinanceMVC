using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Budget;
using PersonalFinanceMVC.Views.Todo;

namespace PersonalFinanceMVC.Controllers

    // TODO: Todo list todos
    // Add subtassks
    // Add a filter
    // Add tags
    // Add a computer reminder (Probably not possible until launched)
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

        [HttpGet("/editTodo")]
        public IActionResult Edit(int id)
        {
            Response.Cookies.Append("EditedTodoId", id.ToString());
            EditVM vm = todoService.CreateEditVM(id);
            return View(vm);
        }

        [HttpPost("/editTodo")]
        public IActionResult Edit(EditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            int id = int.Parse(Request.Cookies["EditedTodoId"]);

            todoService.EditTodo(vm, id);
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

        [HttpPost("/updateStatus")]
        public ActionResult UpdateStatus(int id, string status)
        {
            // Update the status of the issue with the given id
            // using the newStatus value passed in via Ajax.

            todoService.EditStatus(id, status);

            // Return a response indicating success or failure.
            return Json(new { success = true });
        }


    }
}
