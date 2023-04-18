using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Todo;

namespace PersonalFinanceMVC.Controllers
{

    //Add ability to remove todos

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
    }
}
