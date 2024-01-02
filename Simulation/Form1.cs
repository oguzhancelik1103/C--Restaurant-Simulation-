using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation
{
    public partial class Form1 : MaterialSkin.Controls.MaterialForm
    {
        public SimulationEventLogger eventLogger;
        public RestaurantManager restaurantManager;
        public Thread simulationThread;

        public Form1()
        {
            InitializeComponent();

            // Simülasyon olayları için bir SimulationEventLogger örneği oluşturun
            eventLogger = new SimulationEventLogger();

            // Log mesajlarını ListBox'a yazdırmak için event'e bir işleyici ekleyin
            eventLogger.SimulationEventOccurred += LogSimulationEvent;

            // RestaurantManager sınıfını oluşturun
            restaurantManager = new RestaurantManager(eventLogger);
        }

        private void LogSimulationEvent(object sender, string message)
        {
            // GUI thread'i dışında bir thread'den GUI elemanlarına erişim olduğu için
            // Invoke yöntemini kullanarak ListBox'a güncelleme yapın
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() => listBox1.Items.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}")));
            }
            else
            {
                listBox1.Items.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
            }
        }

        private void UpdateUIAfterSimulation()
        {
            // UI elemanlarını güncelleyin (örneğin, ListBox'a yeni müşteri geldi mesajını ekleyin)
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() => listBox1.Items.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Simulation finished.")));
            }
            else
            {
                listBox1.Items.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Simulation finished.");
            }
        }

        private void SimulationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            restaurantManager.StartSimulation();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopSimulation();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartSimulation1();
        }
        private void StartSimulation1()
        {
            if (simulationThread == null || !simulationThread.IsAlive)
            {
                // Simülasyon başlatma işlemleri
                simulationThread = new Thread(() =>
                {
                    // İşlemleri GUI üzerinde göstermek için eventLogger kullanılıyor
                    eventLogger.Log("Simülasyon başlatıldı.");

                    // StartSimulation içindeki işlemleri burada gerçekleştir
                    restaurantManager.StartSimulation();

                    // İşlemlerin tamamlandığını logla
                    eventLogger.Log("Simülation sonlandırıldı.");
                    UpdateUIAfterSimulation();
                });
                simulationThread.Start();
            }
        }
        private void StopSimulation()
        {
            if (simulationThread != null && simulationThread.IsAlive)
            {
                // Simülasyon durdurma işlemleri
                simulationThread.Abort();

                eventLogger.Log("Simülasyon sonlandırıldı.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WaiterForm waiter1 = new WaiterForm();
            waiter1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChefForm chef1 = new ChefForm();
            chef1.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CashierForm cashier1 = new CashierForm();
            cashier1.Show();
            this.Hide();
        }
    }
}
