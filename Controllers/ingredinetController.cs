using Mexican_Restaurant.Data;
using Mexican_Restaurant.Models;
using Mexican_Restaurant.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mexican_Restaurant.Controllers
{
    public class ingredinetController : Controller
    {
        private Repository<Ingredient> ingredinet { get; set; }

        public ingredinetController(ApplicationDbContext context)
        {
            ingredinet = new Repository<Ingredient>(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await ingredinet.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await ingredinet.GetByIdAsync(id, new QueryOption<Ingredient>() { Includes = "ProductIngredients.Product" }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("IngredientId , Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredinet.AddAsync(ingredient);
                return RedirectToAction("Index");
            }

            return View(ingredient);
        }

        [HttpGet]
        public async Task <IActionResult> Delete(int id) 
        {
            return View(await ingredinet.GetByIdAsync(id, new QueryOption<Ingredient> { Includes = "ProductIngredients.Product" }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Ingredient ingredient)
        {
            await ingredinet.DeleteAsync(ingredient.IngredientId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await ingredinet.GetByIdAsync(id, new QueryOption<Ingredient> { Includes = "ProductIngredients.Product" }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ingredient Ingredient)
        {
            if (ModelState.IsValid) 
            { 
            await ingredinet.UpdateAsync(Ingredient);
            return RedirectToAction("Index");
            }
            return View(Ingredient);
        }
    }
}







