using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Simulation.Order;

namespace Simulation
{
    public class Waiter
    {
        public string waiterName { get; set; }
        public SimulationEventLogger eventLogger { get; set; }
        Random random = new Random();
        public List<Order> orders { get; set; } = new List<Order>();
        private readonly object ordersLock = new object();
        public bool IsAvailable { get; set; } = true;
        private RestaurantManager restaurantManager;
        public List<Chef> chefs { get; set; } = new List<Chef>();

        public Waiter()
        {
            // eventLogger özelliğine bir örnek atama
            eventLogger = new SimulationEventLogger();
        }

        public void SeatCustomer(Customer customer, List<Table> tables)
        {
            // Müşteriyi oturt
            if (!customer.IsSeated)
            {
                var availableTables = tables.Where(t => !t.IsOccupied).ToList();

                if (availableTables.Any())
                {
                    Random random = new Random();
                    var randomIndex = random.Next(0, availableTables.Count);
                    var availableTable = availableTables[randomIndex];

                    if (availableTable.Customer == null)
                    {
                        availableTable.IsOccupied = true;
                        customer.IsSeated = true;
                        customer.Table = availableTable;
                        availableTable.Customer = customer; // Masa ile müşteriyi ilişkilendir
                        eventLogger.Log($"Garson {waiterName}, Müşteri {customer.CustomerNumber}'yi Masa {availableTable.TableNumber}'ye oturttu.");
                    }
                    else
                    {
                        eventLogger.Log($"Garson {waiterName}, Masa {availableTable.TableNumber} zaten dolu. Müşteri {customer.CustomerNumber}'yi oturtamadı.");
                    }
                }
                else
                {
                    eventLogger.Log($"Garson {waiterName}, boş masa bulunamadığı için Müşteri {customer.CustomerNumber}'yi oturtamadı.");
                }
            }
            else
            {
                eventLogger.Log($"Garson {waiterName}, Müşteri {customer.CustomerNumber} zaten oturmuş durumda.");
            }
        }


        public void TakeOrder(Customer customer, List<Table> tables)
        {
            lock (ordersLock)
            {
                var orderedTable = tables.FirstOrDefault(t => t.Customer == customer);
                var randomMenuItem = RestaurantMenu.MenuItems[random.Next(RestaurantMenu.MenuItems.Length)];
                var order = new Order(randomMenuItem, customer);
                orderedTable.Orders.Add(order);
                orders.Add(order);
                eventLogger.Log($"Garson {waiterName}, Masa {orderedTable.TableNumber}'den Müşteri {customer.CustomerNumber}'nin siparişini aldı: {randomMenuItem.Name}");
                Thread.Sleep(3000);
                chefs = new List<Chef> { new Chef { chefName = "1" }, new Chef { chefName = "2" } };
                var availableChef = chefs.FirstOrDefault(c => c != null && c.IsAvailable);
                availableChef.Cook(order);
            }
        }


        public void ServeOrder(Order order)
        {
            lock (ordersLock)
            {
                // Burada müşteriye siparişi servis etme işlemleri gerçekleşir.
                // Örneğin, müşteriye yemeği getir, sipariş durumunu güncelle, loglama yap, vb.
                order.Status = Order.OrderStatus.Completed; // Örnek durum güncelleme
                eventLogger.Log($"Garson {waiterName}, Müşteri {order.Customer.CustomerNumber}'ye siparişi servis etti: {order.MenuItem.Name}");
                Thread.Sleep(3000);
                eventLogger.Log($"Müşteri {order.Customer.CustomerNumber} ödemesi alındı.");
                Thread.Sleep(1000);
            }
        }

        public void StartWorking(List<Customer> customers, List<Table> tables)
        {
            List<Thread> threads = new List<Thread>();

            foreach (var customer in customers)
            {
                Thread waiterThread = new Thread(() =>
                {
                    // Her garson için farklı bir müşteri oluştur
                    var currentCustomer = new Customer
                    {
                        // Müşterinin özelliklerini burada ayarla
                    };

                    DoWork(currentCustomer, tables);
                });

                threads.Add(waiterThread);
                waiterThread.Start();
            }

            // Tüm garsonların işini bitirmesini bekleyin
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }


        public void ClearTable(Table table)
        {
            lock (ordersLock)
            {
                // Siparişleri temizle
                table.Orders.Clear();

                // Masayı boşalt
                table.IsOccupied = false;
                table.Customer = null;

                eventLogger.Log($"Garson {waiterName}, Masa {table.TableNumber}'yi temizledi.");
            }
        }

        public void DoWork(Customer customer, List<Table> tables)
        {

            lock (ordersLock)
            {
                    // Müşteriyi oturtma ve sipariş alma işlemlerini gerçekleştir
                SeatCustomer(customer, tables);
                Thread.Sleep(random.Next(1000, 3000));
                TakeOrder(customer, tables);
                Thread.Sleep(random.Next(1000, 3000));
                ServeOrder(orders.First());
                    // Müşteri hesap ödemesi tamamlandıktan sonra masayı temizle
                ClearTable(customer.Table);
            }

        }

    }
}
