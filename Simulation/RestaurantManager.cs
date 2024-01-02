using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulation
{
    public class RestaurantManager
    {
        public List<Waiter> waiters { get; set; } = new List<Waiter>();
        public List<Chef> chefs { get; set; } = new List<Chef>(); 
        public Cashier cashier;
        public List<Table> tables { get; set; } = new List<Table>();
        public List<Customer> customers { get; set; } = new List<Customer>(); 
        private Random random; // Rastgele müşteri eklemek için kullanılacak nesne
        public SimulationEventLogger eventLogger;
        private Action<object, string> LogSimulationEvent;

        private static readonly object lockObject = new object();
        private static RestaurantManager instance;

        public RestaurantManager Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        var eventLogger = new SimulationEventLogger(); // Yeni bir eventLogger oluşturabilirsiniz.
                        instance = new RestaurantManager(eventLogger);
                    }
                    return instance;
                }
            }
        }

        public RestaurantManager(SimulationEventLogger eventLogger)
        {
            // Garsonları, aşçıları, kasiyeri, masaları ve müşterileri oluştur
            waiters = new List<Waiter> { new Waiter { waiterName = "1" }, new Waiter { waiterName = "2" }, new Waiter { waiterName = "3" } };
            chefs = new List<Chef> { new Chef { chefName = "1" }, new Chef { chefName = "2" } };
            cashier = new Cashier { Name = "Kasiyer" };
            tables = new List<Table> { new Table { TableNumber = 1 }, new Table { TableNumber = 2 }, new Table { TableNumber = 3 },
            new Table { TableNumber = 4 }, new Table { TableNumber = 5 }, new Table { TableNumber = 6 } };
            customers = new List<Customer>();
            random = new Random();
            this.eventLogger = eventLogger;
        }

        public void StartSimulation()
        {

            // Müşteri ekleyerek simülasyona devam etmek için sonsuz döngü
            while (true)
            {
                // Bir müşteri eklemeden önce rastgele bir zaman aralığını bekleyin
                int waitTime = random.Next(1000, 3000); // Miliseconds
                Thread.Sleep(waitTime);

                // Yeni müşteri ekleme
                var newCustomer = new Customer { CustomerNumber = customers.Count + 1 };
                customers.Add(newCustomer);

                eventLogger.Log($"Yeni müşteri geldi! Müşteri {newCustomer.CustomerNumber}");

                Thread.Sleep(1000);

                // Müsait olan garsonları filtrele
                var availableWaiters = waiters.Where(w => w.IsAvailable).ToList();

                if (availableWaiters.Any())
                {
                    // Rastgele bir müsait garsonu seç
                    Random random = new Random();
                    var randomIndex = random.Next(0, availableWaiters.Count);
                    var selectedWaiter = availableWaiters[randomIndex];

                    // Seçilen garsona müşteriyi oturtma ve sipariş alma işlemlerini yaptırın
                    selectedWaiter.DoWork(newCustomer, tables);
                }
                else
                {
                    eventLogger.Log("Müsait garson bulunamadı.");
                }
            }
            }
            // Bu noktaya asla ulaşılmayacaktır.
            // SimulationEventLogger.Log("Restoran simülasyonu tamamlandı.");
        }
}
