using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class Table
    {
        public int TableNumber { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public bool IsOccupied { get; set; }
        public Customer Customer { get; set; }
    }

}
