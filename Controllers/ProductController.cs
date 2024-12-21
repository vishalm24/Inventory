using Inventory.Data;
using Inventory.IServices;
using Inventory.Models;
using Inventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Inventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index(int pg = 1)
        {
            var indexData = _productRepository.Index(pg);
            this.ViewBag.Pager = indexData.pager;
            return View(indexData.data);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            bool isDuplicate = _productRepository.AddCategory(product);
            if (isDuplicate)
            {
                ViewData["Message"] = ("A product with the same name already exists");
                return View("AddProduct");
            }
            TempData["success"] = "Product has been created!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            Product product = _productRepository.Update(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            _productRepository.Update(product);
            TempData["success"] = "Product has been updated!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            string message = _productRepository.Delete(id);
            if(message.Equals("Not Found"))
            {
                return NotFound();
            }
            else if (message.Equals("Success"))
            {
                TempData["success"] = "Product has been sucessfully deleted!";
            }
            else if (message.Equals("Bad Request"))
            {
                return BadRequest();
            }
            return RedirectToAction("Index");
        }
    }
}
