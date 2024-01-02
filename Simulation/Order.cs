using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class Order
    {
        public enum OrderStatus
        {
            Pending,
            InProgress,
            Completed,
            Canceled
        }

        public MenuItem MenuItem { get; set; }
        public OrderStatus Status { get; set; }
        public Customer Customer { get; set; } // Add this line

        // Constructor with Customer parameter
        public Order(MenuItem menuItem, Customer customer)
        {
            MenuItem = menuItem;
            Customer = customer;
            Status = OrderStatus.Pending; // Set the initial status, for example, to Pending
        }
    }

}
