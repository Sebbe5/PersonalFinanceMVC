using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Todo;

namespace PersonalFinanceMVC.Controllers

    // TODO: Add deadlines
    // TODO: Add the ability to edit an existing todo
    // TODO: Add categories
    // TODO: Add a search function
    // TODO: Add tags
    // TODO: Add comments
    // TODO: Add subtassks
    // TODO: Add the ability to sort the list in different orders
    // TODO: Add a computer reminder (Probably not possible until launched
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
        public IActionResult Delete(string item)
        {
            todoService.DeleteTodo(item);
            return RedirectToAction(nameof(TodoList));
        }


    }
}
