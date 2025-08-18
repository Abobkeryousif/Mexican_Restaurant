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
    }
}
