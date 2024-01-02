using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class Customer
    {
        public int CustomerNumber { get; set; }
        public bool IsSeated { get; set; }
        public Table Table { get; set; }
    }

}
