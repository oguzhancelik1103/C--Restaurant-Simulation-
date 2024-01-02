using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation
{
    public partial class ChefForm : MaterialSkin.Controls.MaterialForm
    {
        public ChefForm()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 restaurant1 = new Form1();
            restaurant1.Show();
            this.Hide();
        }
    }
}
