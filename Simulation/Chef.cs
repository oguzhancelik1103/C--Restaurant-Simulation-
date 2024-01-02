using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Simulation.Order;

namespace Simulation
{
    public class Chef
    {
        public string chefName { get; set; }
        public List<Order> orders { get; set; } = new List<Order>();
        private readonly object ordersLock = new object();
        public bool IsAvailable { get; set; } = true; // Varsayılan olarak tüm şefler müsait
        public static List<Chef> Chefs { get; set; } // static chefs koleksiyonu
        public SimulationEventLogger eventLogger {  get; set; }
        public List<Chef> chefs { get; set; } = new List<Chef>();
        
        public Chef()
        {
            // eventLogger özelliğine bir örnek atama
            eventLogger = new SimulationEventLogger();
        }

        public Chef GetAvailableChef()
        {
            // Burada istediğiniz koşullara göre availableChef'i bulun
            // Örnek olarak, aşağıdaki kod satırı tüm şefler arasından ilk müsait olanı seçer
            var availableChef = chefs.FirstOrDefault(c => c != null && c.IsAvailable);

            return availableChef;
        }

        public void Cook(Order order)
        {
            lock (ordersLock)
            {
                chefs = new List<Chef> { new Chef { chefName = "1" }, new Chef { chefName = "2" } };
                var availableChef = GetAvailableChef();
                if (availableChef == null)
                {
                    eventLogger.Log($"Müsait aşçı bulunamadı. Sipariş hazırlanamıyor.");
                    return;
                }

                eventLogger.Log($"{availableChef.chefName} aşçısı, Masa {order.Customer.Table.TableNumber} siparişini hazırlıyor...");

                // Simüle edilen pişirme süresi
                Thread.Sleep(TimeSpan.FromSeconds(5));

                eventLogger.Log($"Masa {order.Customer.Table.TableNumber} siparişi hazır!");

                order.Status = Order.OrderStatus.Completed;
                availableChef.IsAvailable = false; // Aşçı artık meşgul durumda
            }
        }
    }
}
