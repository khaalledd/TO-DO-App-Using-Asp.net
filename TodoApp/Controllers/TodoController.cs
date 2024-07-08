using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.ViewModels;
using TodoAPP.Context;

namespace TodoApp.Controllers
{
    [Authorize(Policy ="User")]
    public class TodoController : Controller
    {
        private readonly TodoContext context;

        public TodoController(TodoContext context)
        {
            this.context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var todos = context.Todos.Include(t => t.Category).ToList();
            return View(todos);
        }

        // Creating
        public IActionResult NewTodo()
        {
            var model = new CreateTodo() { Id = 0 }; // Explicitly set Id to 0 for new todo
            ViewData["Cate"] = context.Categories.ToList();
            return View(model);
        }

        public IActionResult SaveNewTodo(CreateTodo todo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Cate"] = context.Categories.ToList();
                return View("NewTodo", todo);
            }

            var newTodo = new Todo()
            {
                Name = todo.Name,
                Description = todo.Description,
                CategoryId = todo.CategoryId,
                IsDone = false,
                CreatedAt = DateTime.Now,
            };

            context.Todos.Add(newTodo);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Updating
        public IActionResult UpdateTodo(int id)
        {
            var todo = context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            var model = new CreateTodo()
            {
                Id = id,
                Name = todo.Name,
                Description = todo.Description,
                CategoryId = todo.CategoryId,
            };

            ViewData["Cate"] = context.Categories.ToList();
            return View("NewTodo", model);
        }

        public IActionResult SaveUpdateTodo(CreateTodo todo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Cate"] = context.Categories.ToList();
                return View("NewTodo", todo);
            }

            var editedTodo = context.Todos.FirstOrDefault(t => t.Id == todo.Id);
            if (editedTodo == null)
            {
                return NotFound();
            }

            editedTodo.Name = todo.Name;
            editedTodo.Description = todo.Description;
            editedTodo.CategoryId = todo.CategoryId;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Deleting
        public IActionResult DeleteTodo(int id)
        {
            var todo = context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            context.Todos.Remove(todo);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ToggleTodo(int id) 
        {
            var todo = context.Todos.FirstOrDefault(t=>t.Id == id);
            todo.IsDone = !todo.IsDone;
            context.SaveChanges();
            return RedirectToAction("Index");
        
        }
    }
}
