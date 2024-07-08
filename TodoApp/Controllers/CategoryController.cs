using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.ViewModels;
using TodoAPP.Context;

namespace TodoAPP.Controllers
{
    public class CategoryController : Controller
    {
        private readonly TodoContext context;

        public CategoryController()
        {
            context = new TodoContext();
        }

        //Reading
        public IActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }

        //Creating
        public IActionResult NewCategory()
        {
            var model = new CreateCategory() { Id = 0 }; // Explicitly set Id to 0 for new category
            return View(model);
        }

        public IActionResult SaveNewCategory(CreateCategory category)
        {
            if (ModelState.IsValid)
            {
                var newCate = new Category()
                {
                    Name = category.Name,
                };

                context.Categories.Add(newCate);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("NewCategory", category);
        }

        //Updating
        public ActionResult UpdateCategory(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return View("Invalid", new InvalidOperation() { Message=$"Category with {id} Not Found!"});
            }

            var model = new CreateCategory()
            {
                Name = category.Name,
                Id = id
            };

            return View("NewCategory", model);
        }

        public IActionResult SaveUpdateCategory(CreateCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View("NewCategory", category);
            }

            var cate = context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (cate == null)
            {
                return NotFound();
            }

            cate.Name = category.Name;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Deleting
        public IActionResult DeleteCategory(int id)
        {
            var cate = context.Categories.FirstOrDefault(c => c.Id == id);
            if (cate == null)
            {
                return NotFound();
            }

            context.Remove(cate);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
