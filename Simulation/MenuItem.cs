using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class MenuItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }


    public static class RestaurantMenu
    {
        public static MenuItem[] MenuItems { get; private set; }

        static RestaurantMenu()
        {
            // Initialize the menu with 5 items
            MenuItems = new MenuItem[]
            {
                new MenuItem { Name = "Köfte", Price = 20.0m },
                new MenuItem { Name = "Izgara Tavuk", Price = 18.5m },
                new MenuItem { Name = "Makarna", Price = 15.0m },
                new MenuItem { Name = "Sebzeli Pilav", Price = 17.5m },
                new MenuItem { Name = "Çorba", Price = 12.0m }
            };
        }
    }
}

