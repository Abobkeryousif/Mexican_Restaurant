using Mexican_Restaurant.Data;
using Mexican_Restaurant.Models;
using Mexican_Restaurant.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mexican_Restaurant.Controllers
{
    public class productController : Controller
    {
        private Repository<Product> _product;
        private Repository<Ingredient> _ingredient;
        private Repository<Category> _category;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public productController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _product = new Repository<Product>(context);
            _ingredient = new Repository<Ingredient>(context);
            _category = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View (await _product.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await _ingredient.GetAllAsync();
            ViewBag.Categories = await _category.GetAllAsync();

            if (id == 0) 
            {
                ViewBag.Operation = "Add";
                return View(new Product());
            }
            else
            {
                Product product = await _product.GetByIdAsync(id,new QueryOption<Product> {
                    Includes = "ProductIngredients.Ingredien, Category"
                });
                ViewBag.Operation = "Edit"; 
                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(Product product , int[] ingredientIds,int catId)
        {
            ViewBag.Ingredients = await _ingredient.GetAllAsync();
            ViewBag.Categories = await _category.GetAllAsync();

            if (ModelState.IsValid)
            {

                if(product.ImageFile != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = uniqueFileName;
                }

                if(product.ProductId == 0)
                {
                    
                    product.CategoryId = catId;

                    foreach (int id in ingredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }

                    await _product.AddAsync(product);
                    return RedirectToAction("Index", "product");
                }
                
                else
                {
                    var existProduct = await _product.GetByIdAsync(product.ProductId, new QueryOption<Product> {Includes = "ProductIngredients"});
                    if(existProduct == null)
                    {
                        ModelState.AddModelError("","Product Not Found");
                        ViewBag.Ingredients = await _ingredient.GetAllAsync();
                        ViewBag.Categories = await _category.GetAllAsync();
                        return View(product);
                    }

                    existProduct.Name = product.Name;
                    existProduct.Description = product.Description;
                    existProduct.Price = product.Price;
                    existProduct.Stock = product.Stock;
                    existProduct.CategoryId = catId;

                    existProduct.ProductIngredients?.Clear();
                    foreach(int id in ingredientIds) 
                    { 
                        existProduct.ProductIngredients?.Add(new ProductIngredient {IngredientId = id ,ProductId = product.ProductId});
                    }

                    try
                    {
                        await _product.UpdateAsync(existProduct);
                    }

                    catch(Exception ex)
                    {
                        ModelState.AddModelError("",$"Error: {ex.GetBaseException().Message}");
                        ViewBag.Ingredients = await _ingredient.GetAllAsync();
                        ViewBag.Categories = await _category.GetAllAsync();
                        return View(product);
                    }
                }
            }
            return RedirectToAction("Index" , "product");
        }

        [HttpPost] 
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _product.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("","Product Not Found");
                return RedirectToAction("Index");
            }
        }
    }
}







