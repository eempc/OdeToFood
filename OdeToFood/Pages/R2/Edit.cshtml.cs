using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;
using OdeToFood.Data;

// Drop down picker select binding is in 3 parts:
// (1) the htmlHelper which needs 3 components: the private field declaration, the constructor initialisation and the OnGet() population
// (2) The IEnumerable<SelectListItem>  
// (3) the cshtml needs asp-items="Model.Cuisines" in the picker

namespace OdeToFood.Pages.R2 {
    public class EditModel : PageModel {
        private readonly OdeToFood.Data.OdeToFoodDbContext _context;

        public EditModel(OdeToFood.Data.OdeToFoodDbContext context, IHtmlHelper htmlHelper) {
            _context = context;
            this.htmlHelper = htmlHelper;
        }

        private readonly IHtmlHelper htmlHelper;

        public IEnumerable<SelectListItem> Cuisines { get; set; }

        [BindProperty]
        public Restaurant Restaurant { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
           
            if (id == null) {
                return NotFound();
            }

            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(m => m.Id == id);

            if (Restaurant == null) {
                return NotFound();
            }

            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(Restaurant).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!RestaurantExists(Restaurant.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RestaurantExists(int id) {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
