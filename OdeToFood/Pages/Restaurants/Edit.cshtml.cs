using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Data;
using OdeToFood.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OdeToFood.Pages.Restaurants {
    public class EditModel : PageModel {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        // Data binding here, it is bound to anything with asp-for="Restaurant.{Property}"
        [BindProperty]
        public Restaurant Restaurant { get; set; }

        // For the <select> picker, it has been bound to asp-items="Model.Cuisuines"
        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IRestaurantData restaurantData, IHtmlHelper htmlHelper) {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }

        // OnGet is when the page is requested, the id is in the URL, the restaurant id is passed as an argument by the cshtml first line
        public IActionResult OnGet(int? restaurantId) {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>(); // Data binding for the drop down select, look at the htmlHelper that populates the variable

            // If null, add new restaurant, otherwise edit existing restaurant
            if (restaurantId.HasValue) {
                Restaurant = restaurantData.GetByID(restaurantId.Value);
            } else {
                Restaurant = new Restaurant();                
            }

            if (Restaurant == null) {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        // OnPost for editing/adding a restaurant
        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>(); // Data binding for the drop down select, look at the htmlHelper
                return Page();
            }

            if (Restaurant.Id > 0) {
                Restaurant = restaurantData.Update(Restaurant);
            } else {
                Restaurant = restaurantData.Add(Restaurant);
            }
            
            restaurantData.Commit();
            TempData["Message"] = "Restaurant saved!"; // This isn't inserted into the method below, it is a special dictionary
            // Arguments here: redirect to the detail page, with a new anonymous method of the ID
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
        }
    }
}