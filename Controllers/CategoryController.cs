using Inventory.Data;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationContext context;
        public CategoryController(ApplicationContext context) {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await context.Categories.ToListAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {
            Category category = new Category();
            if (id != null && id != 0)
            {
                category = await context.Categories.FindAsync(id);
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            context.Categories.Update(category);
            TempData["success"] = "Category has been updated!";
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                bool status = context.Products.Any(x => x.CategoryId == id);
                if (status)
                {
                    TempData["warning"] = "Category is taken by another product!";
                }
                else
                {
                    var category = await context.Categories.FindAsync(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        context.Categories.Remove(category);
                        await context.SaveChangesAsync();
                        TempData["success"] = "Categpry has been sucessfully deleted!";

                    }
                }
            }
            else
            {
                return BadRequest();
            }
            return RedirectToAction("Index");
        }
    }
}
