using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using OdeToFood.Core;
using OdeToFood.Data;
using System.Collections.Generic;

namespace OdeToFood.Pages.Restaurants {
    // Note the names of the classes
    public class ListModel : PageModel {
        public string Message { get; set; }
        public string ConfigMessage { get; set; }

        // Binding for get request, look for the element with asp-for="SearchTerm"
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        // Collection of restaurants
        public IEnumerable<Restaurant> Restaurants { get; set; }

        // Services, connecting to the config file, and connecting to the restaurant data memory "database" prior to implementing SQL db
        private readonly IConfiguration _config;
        private readonly IRestaurantData _restaurantData;

        public ListModel(IConfiguration config, IRestaurantData restaurantData) {
            _config = config;
            _restaurantData = restaurantData;
        }

        // @Model is a unique keyword
        // OnGet, you could put an argument in like string searchTerm which would correspond to the element with name="searchTerm"
        public void OnGet() {
            Message = "Hello World"; // Just a random variable
            ConfigMessage = _config["Config Message"]; // Automatic JSON deserialisation into dictionary
           
            //Restaurants = _restaurantData.GetAll();
            Restaurants = _restaurantData.GetRestaurantsByName(SearchTerm);
        }
    }
}