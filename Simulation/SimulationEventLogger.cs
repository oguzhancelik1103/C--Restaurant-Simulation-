using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class SimulationEventLogger
    {
        // Yeni eklenen event
        public event EventHandler<string> SimulationEventOccurred;

        public void Log(string message)
        {
            // Loglama işlemleri burada gerçekleştirilir
            Console.WriteLine($"[Log] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");

            // Yeni eklenen event'i tetikle
            SimulationEventOccurred?.Invoke(this, message);
        }
    }
}
