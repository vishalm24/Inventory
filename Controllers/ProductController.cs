using Inventory.Data;
using Inventory.Models;
using Inventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Inventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationContext context;
        public ProductController(ApplicationContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int pg = 1)
        {
            ////Instance of ProductCategoryList.cs from ViewModel because we can't share multiple data in return.
            ////That's why we needed to create ProductCategoryList
            //ProductCategoryList prod = new ProductCategoryList();
            //prod.Products = context.Products.ToList();
            //prod.Categories = context.Categories.ToList();
            //return View(prod);
            List<Product> products = context.Products.ToList();
            const int pageSize = 10;
            if (pg < 1) 
                pg = 1;
            int recsCount = products.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = (from p in context.Products
                        join c in context.Categories
                        on p.CategoryId equals c.CategoryId
                        select new ProductCategorySummaryViewModel
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName
                        }).Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {
            Product product = new Product();
            if (id != null && id != 0)
            {
                product = await context.Products.FindAsync(id);
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            context.Products.Update(product);
            TempData["success"] = "Product has been updated!";
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            bool isDuplicate = await context.Products
                .AnyAsync(p => p.ProductName == product.ProductName);
            if (isDuplicate)
            {
                ViewData["Message"] = ("A product with the same name already exists");
                return View("AddProduct");
            }
            await context.Products.AddAsync(product);
            TempData["success"] = "Product has been created!";
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var product = await context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    TempData["success"] = "Product has been sucessfully deleted!";
                    
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
