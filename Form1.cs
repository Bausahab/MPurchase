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
    public partial class FrmPurchase : Form
    {
        UI.ucHead head;
        public FrmPurchase()
        {
            InitializeComponent();
            head = new UI.ucHead();
            DBCONNECT.myWebConnServer();
            DBCONNECTNEW.myWebConnServer();
            globalvalues.Uid = "1"; globalvalues.BranchID = "1"; globalvalues.Uname = "ADMIN";
            EXTRA.GENERATEEVENT(this);
            globalvalues.myTab(this);
        }

        void initadd()
        {
            DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(slno) IS NULL THEN 0 ELSE MAX(slno) END AS 'VALUE' FROM cash_receipt with (tablockx)");
            LBLLastNo.Text = dr1[0].ToString();
            DataTable dttname = DBCONNECT.ExecuteDataTable("Select id,accountname as name From Accounts Where subledgerid=(Select refvalue From Reference1 Where code=8) Order By accountname");   //for transport
            CommonFunction.bindCombobox(dttname, "id", "name", "Select", CBTransport);
            DataTable dtPname = DBCONNECT.ExecuteDataTable("Select id,accountname as name From Accounts Where subledgerid=(Select refvalue From Reference1 Where code=1) Order By accountname");//party
            CommonFunction.bindCombobox(dtPname, "id", "name", "Select", CBParty);
            DataTable dtBname = DBCONNECT.ExecuteDataTable("Select id,accountname as name From Accounts Where subledgerid=(Select refvalue From Reference1 Where code=2) Order By accountname");  // Broker
            CommonFunction.bindCombobox(dtBname, "id", "name", "Select", CBBroker);
            DataTable dtBKname = DBCONNECT.ExecuteDataTable("Select id,accountname as name From Accounts Where accounttypeid=(Select refvalue From Reference1 Where code=45) Order By accountname");  // Bank
            CommonFunction.bindCombobox(dtBKname, "id", "name", "Select", CBBank);
            DataTable dtSaudaname = DBCONNECT.ExecuteDataTable("Select id,accountname as name From Accounts Where subledgerid=(Select refvalue From Reference1 Where code=1) Order By accountname");  // Sauda For
            CommonFunction.bindCombobox(dtSaudaname, "id", "name", CBSaudafor);
            DataTable dtCommodity = DBCONNECT.ExecuteDataTable("Select id,ITEM_NAME AS NAME from ITEM where GROUP_TYPE_ID=3");  // Commodity For
            CommonFunction.bindCombobox(dtCommodity, "id", "name", "Select", CBCommodity);

            //--Payment By
            DataTable Dtpay = new DataTable();
            Dtpay.Columns.Add("ID", typeof(int));
            Dtpay.Columns.Add("NAME", typeof(string));

            DataRow drpay1 = Dtpay.NewRow();
            drpay1[0] = 1;
            drpay1[1] = "CASH";
            Dtpay.Rows.InsertAt(drpay1, 0);

            DataRow drpay2 = Dtpay.NewRow();
            drpay2[0] = 2;
            drpay2[1] = "BANK";
            Dtpay.Rows.InsertAt(drpay2, 1);

            CommonFunction.bindCombobox(Dtpay, "ID", "NAME", "Select", CBPy);

            //Payment End


            // DataTable dtBRname = DBCONNECT.ExecuteDataTable("Select id,accountname as name From Accounts Where subledgerid=(Select refvalue From Reference1 Where code=2) Order By accountname");  // Branch
            // CommonFunction.bindCombobox(dtBRname, "id", "name", CBB);



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //disable purchase button untill all lots enterd.. get lots of this purchase..
            this.tableLayoutPanelBase.Controls.Add(head, 0, 0);
            head.Show();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
    }
}
