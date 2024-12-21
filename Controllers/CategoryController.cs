using Inventory.Data;
using Inventory.IServices;
using Inventory.Models;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Index(int pg = 1)
        {
            var indexData = _categoryRepository.Index(pg);
            this.ViewBag.Pager = indexData.pager;
            return View(indexData.data);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            bool isDuplicate = _categoryRepository.AddCategory(category);
            if (isDuplicate)
            {
                ViewData["Message"] = ("A category with the same name already exists");
                return View("AddCategory");
            }
            TempData["success"] = "Category has been created!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            Category category = _categoryRepository.Update(id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            _categoryRepository.Update(category);
            TempData["success"] = "Category has been updated!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            string message = _categoryRepository.Delete(id);
            if(message.Equals("Category with Products"))
            {
                TempData["warning"] = "Category with Products has been deleted";
            }
            else if(message.Equals("Not Found"))
            {
                return NotFound();
            }
            else if (message.Equals("Success"))
            {
                TempData["success"] = "Category has been sucessfully deleted!";
            }
            else if(message.Equals("Bad Request"))
            {
                return BadRequest();
            }
            return RedirectToAction("Index");
        }
    }
}
