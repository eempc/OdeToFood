using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants {
    public class DetailModel : PageModel {
        private readonly IRestaurantData restaurantData;
        public Restaurant Restaurant { get; set; }
        [TempData] // TempData["Message"]
        public string Message { get; set; }

        // Rename from void to IActionResult
        public IActionResult OnGet(int restaurantId) {
            Restaurant = restaurantData.GetByID(restaurantId);
            if (Restaurant == null) {
                return RedirectToPage("./NotFound"); // Redirect if null
            }
            return Page(); // Return the page that is built in the cshtml file
        }

        public DetailModel(IRestaurantData restaurantData) {
            this.restaurantData = restaurantData;
        }
    }
}