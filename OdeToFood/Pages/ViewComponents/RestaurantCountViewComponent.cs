using Microsoft.AspNetCore.Mvc;
using OdeToFood.Data;

namespace OdeToFood.Pages.ViewComponents {
    public class RestaurantCountViewComponent : ViewComponent {
        private readonly IRestaurantData restaurantData;

        // Something about dependency injection
        public RestaurantCountViewComponent(IRestaurantData restaurantData) {
            this.restaurantData = restaurantData;
        }

        // Render view, with a count argument, this will display on every page that uses it, i.e. the Layout
        public IViewComponentResult Invoke() {
            int count = restaurantData.GetCountOfRestaurants();
            return View(count);
        }
    }
}
