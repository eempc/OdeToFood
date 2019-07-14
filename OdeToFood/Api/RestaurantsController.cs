using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;
using OdeToFood.Data;

// This is a full EF MVC API controller chosen with the model Restaurant
// It allows for API requests, check out the GET request in order to see the JSON get method
// The ClientRestaurant.cshtml uses jquery to download the JSON, but you can use Angular or React or whatever
namespace OdeToFood.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase {
        private readonly OdeToFoodDbContext _context;

        public RestaurantsController(OdeToFoodDbContext context) {
            _context = context;
        }

        // e.g. GET: api/Restaurants -> type this in after localhost and you will get a json of all restaurants - READ THIS!!!!!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants() {
            return await _context.Restaurants.ToListAsync();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id) {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null) {
                return NotFound();
            }

            return restaurant;
        }

        // e.g. Update: PUT: api/Restaurants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(int id, Restaurant restaurant) {
            if (id != restaurant.Id) {
                return BadRequest();
            }

            _context.Entry(restaurant).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!RestaurantExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Restaurants
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurant(Restaurant restaurant) {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRestaurant", new { id = restaurant.Id }, restaurant);
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Restaurant>> DeleteRestaurant(int id) {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return restaurant;
        }

        private bool RestaurantExists(int id) {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
