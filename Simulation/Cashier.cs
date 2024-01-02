using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulation
{
    public class Cashier
    {
        public string Name { get; set; }
        public SimulationEventLogger eventLogger;

        public void GenerateBill(Table table)
        {
            eventLogger.Log($"Hesap Oluşturuluyor - Masa {table.TableNumber}");

            // Multithreading kullanarak her bir siparişi ayrı bir iş parçacığında işle
            List<Thread> threads = new List<Thread>();
            foreach (var order in table.Orders)
            {
                Thread generateBillThread = new Thread(() =>
                {
                    eventLogger.Log($"{order.MenuItem.Name}: {order.MenuItem.Price:C}");
                    Thread.Sleep(1000); // Simüle edilen işlem süresi
                });

                threads.Add(generateBillThread);
                generateBillThread.Start();
            }

            // Tüm iş parçacıklarının tamamlanmasını bekleyin
            foreach (var thread in threads)
            {
                thread.Join();
            }

            decimal totalBill = table.Orders.Sum(order => order.MenuItem.Price);

            eventLogger.Log($"Toplam Hesap: {totalBill:C}");
            eventLogger.Log($"Hesap, {table.TableNumber} numaralı masa için oluşturuldu. Ödeme bekleniyor.");

            // Ödeme işlemleri
            AcceptPayment(table, totalBill);
        }

        public void AcceptPayment(Table table, decimal amountPaid)
        {
            decimal totalBill = table.Orders.Sum(order => order.MenuItem.Price);

            if (amountPaid >= totalBill)
            {
                decimal change = amountPaid - totalBill;
                eventLogger.Log($"Ödeme başarıyla alındı. {change:C} tutarında para üstü verilecek.");
                // Müşteriye para üstü verilebilir veya başka bir işlem yapılabilir.
            }
            else
            {
                eventLogger.Log($"Ödeme tutarı yetersiz. Hesap ödenemedi.");
            }
        }
    }
}
