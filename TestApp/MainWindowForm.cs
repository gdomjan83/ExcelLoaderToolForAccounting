using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp {
    public partial class MainWindowForm : Form {
        public MainWindowForm() {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            MessageBox.Show("test");
        }
    }
}
