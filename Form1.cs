using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MPurchase
{
    public partial class Form1 : Form
    {
        UI.ucHead head;
        public Form1()
        {
            InitializeComponent();
            head = new UI.ucHead();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            this.tableLayoutPanelBase.Controls.Add(head, 0, 0);
            head.Show();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
    }
}
