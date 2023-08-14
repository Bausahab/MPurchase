using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPurchase.UI
{
    public partial class ucHead : UserControl
    {
        LogicModels.PurchaseHead LastPurchase;
        string TimeSinceLastPurchase = string.Empty;
        string LastPurchasedetailsaddress = "DefaultAddressHereOrAppConfig";
        public ucHead()
        {
            InitializeComponent();
        }

        private void ucHead_Load(object sender, EventArgs e)
        {

        }
    }
}

