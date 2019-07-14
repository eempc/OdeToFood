using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Data;
using OdeToFood.Core;

namespace OdeToFood.Pages.Restaurants {
    public class DeleteModel : PageModel {
        private readonly IRestaurantData restaurantData;
        public Restaurant Restaurant { get; set; }

        public DeleteModel(IRestaurantData restaurantData) {
            this.restaurantData = restaurantData;
        }

        public IActionResult OnGet(int restaurantId) {
            Restaurant = restaurantData.GetByID(restaurantId);
            if (Restaurant == null) {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost(int restaurantId) {
            Restaurant restaurant = restaurantData.Delete(restaurantId); // Delete function is here
            restaurantData.Commit();
            if (restaurant == null) {
                return RedirectToPage("./NotFound");
            }
            TempData["Message"] = $"{restaurant.Name} deleted";
            return RedirectToPage("./List");
        }
    }
}