using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparePartsMagazine.Models;
using SparePartsMagazine.Services;

namespace SparePartsMagazine.Controllers
{
    public class PartsController : Controller
    {

        private readonly PartsService _partsService;
        private readonly ApplicationDbContext _context;
        public PartsController(ApplicationDbContext context)
        {
            _context = context;
            _partsService = new PartsService(context);
        }

        
        public async Task<IActionResult> Index(string sortBy, string nameSearch, string priceSearch)
        {
            ViewData["CurrentSort"] = sortBy;
            ViewData["NameSortParm"] =
            String.IsNullOrEmpty(sortBy) ? "Name_desc" : "";
            ViewData["PriceSortParm"] =
            sortBy == "Price" ? "Price_desc" : "Price";
            ViewData["InputDateSortParm"] =
            sortBy == "InputDate" ? "InputDate_desc" : "InputDate";
            ViewData["ModificationDateSortParm"] =
            sortBy == "ModificationDate" ? "ModificationDate_desc" : "ModificationDate";

            ViewData["NameSearch"] = nameSearch;
            ViewData["PriceSearch"] = priceSearch;

            var parts = await _partsService.GetAllFiltered(sortBy, nameSearch, priceSearch);
            return View(parts);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Part part)
        {
            if (ModelState.IsValid)
            {
                var CreatedPart = await _partsService.CreateAsync(part);
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }
            return View(part);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Part part)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var movie = await _partsService.UpdateAsync(id, part);
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            var isDeleted = await _partsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
