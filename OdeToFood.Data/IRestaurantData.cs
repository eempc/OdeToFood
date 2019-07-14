using System;
using System.Collections.Generic;
using System.Text;
using OdeToFood.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OdeToFood.Data {
    public interface IRestaurantData {
        IEnumerable<Restaurant> GetAll();
        IEnumerable<Restaurant> GetRestaurantsByName(string name);
        Restaurant GetByID(int id);
        Restaurant Update(Restaurant updatedRestaurant);
        Restaurant Delete(int id);
        int Commit();
        Restaurant Add(Restaurant newRestaurant);
        int GetCountOfRestaurants();
    }

    // This should go into a new file by convention. Since I made the SQL CRUD after the in memory CRUD, see how an interface is useful for both?
    // These are not async
    public class SqlRestaurantData : IRestaurantData {
        private readonly OdeToFoodDbContext db;

        public SqlRestaurantData(OdeToFoodDbContext db) {
            this.db = db;
        }

        public Restaurant Add(Restaurant newRestaurant) {
            db.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit() {
            return db.SaveChanges();
        }

        public Restaurant Delete(int id) {
            Restaurant restaurant = GetByID(id);
            if (restaurant != null) {
                db.Restaurants.Remove(restaurant); // The table has been specified here but not in the Add()?
            }
            return restaurant;
        }

        public IEnumerable<Restaurant> GetAll() {
            throw new NotImplementedException();
        }

        public Restaurant GetByID(int id) {
            return db.Restaurants.Find(id); // Jeez find by id instead of the SELECT * nonsense, can't believe I wasted all my time learning SQL strings
        }

        public int GetCountOfRestaurants() {
            return db.Restaurants.Count();
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name) {
            var query = from r in db.Restaurants
                        where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Restaurant Update(Restaurant updatedRestaurant) {
            var entity = db.Restaurants.Attach(updatedRestaurant); // Attach is update
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }
    }

    // This should go into a new file by convention
    public class InMemoryRestaurantData : IRestaurantData {
        List<Restaurant> restaurants;

        public InMemoryRestaurantData() {
            restaurants = new List<Restaurant>() {
                new Restaurant { Id = 1, Name = "Ralph's Pasta", Location = "Rome", Cuisine = CuisineType.Italian},
                new Restaurant { Id = 2, Name = "Bart's Gyros", Location = "Athens", Cuisine = CuisineType.Greek},
                new Restaurant { Id = 3, Name = "Homer's Curry", Location = "Mumbai", Cuisine = CuisineType.Indian},
                new Restaurant { Id = 4, Name = "Lisa's Fry Up", Location = "London", Cuisine = CuisineType.British},
                new Restaurant { Id = 5, Name = "Milhouse's Fajitas", Location = "Mexico", Cuisine = CuisineType.Mexican},
            };
        }

        public Restaurant Add(Restaurant newRestaurant) {
            restaurants.Add(newRestaurant);
            newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
            return newRestaurant;
        }

        public int Commit() {
            return 0;
        }

        public Restaurant Delete(int id) {
            Restaurant restaurant = restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null) {
                restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public IEnumerable<Restaurant> GetAll() {
            return from r in restaurants
                   orderby r.Name
                   select r;
        }

        public Restaurant GetByID(int id) {
            return restaurants.SingleOrDefault(r => r.Id == id);
        }

        public int GetCountOfRestaurants() {
            return restaurants.Count();
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null) {
            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }

        public Restaurant Update(Restaurant updatedRestaurant) {
            Restaurant restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if (restaurant != null) {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine;
            }
            return restaurant;
        }
    }
}
