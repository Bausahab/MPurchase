using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPurchase.LogicModels
{
    /// <summary>
    /// Manish Raj Singh@082023
    /// manishrajsingh@live.in
    /// </summary>
    /// public modifire may replace with internal and private;
    public class ComodityPurchase : IPurchase
    {
        public int PurchaseNo { get; set; }
        public string PurchaseTrno { get; set; }
        public DateTime PrchaseDate { get; set; }
        public string PurchaseName { get; set; }//Required for tagging,grouping and UI.
        public string PartyName { get; set; }
        public string BrokerName { get; set; }

        public List<PurchaseLot> Lots = new List<PurchaseLot>();

    }
    public class PurchaseLot
    {
        public int LotId { get; set; }
        public int LotNo { get; set; }//To Extend Application to the lab reports...
        public string LotName { get; set; }
        public string Sauda { get; set; }
        public LotCondition Condition { get; set; }
        public LotBill Bill { get; set; }

        //A Transport may have load of different kinds of objects with them..
        //there is an extention:  Different brokers and parties can be seen in reports...
        //While Transportation was single..and they are devided into lots..

        //public string PartyName { get; set; }
        //public string BrokerName { get; set; }
        //public PurchaseCondition Condition { get; set; }
    }
    public class LotBill
    {
        public int itemId { get; set; }
        public string ItemName { get; set; }
        public decimal SaudaRate { get; set; } = 00M;
        public decimal BillWeight { get; set; }
        public decimal Rate { get; set; } = 00M;
        public decimal Amount { get; set; } = 00M;
        public decimal CurrentRate { get; set; } = 00M;
        public int Bags { get; set; }
        public decimal OtherAmount { get; set; }
        public decimal GstAmount { get; set; } = 00M;
        public decimal BillAmount { get; set; } = 00M;
    }
    public class LotCondition
    {
        public decimal Oil { get; set; } = 00M;
        public decimal Ffa { get; set; } = 00M;
    }
    public class PurchaseTransport
    {

    }
}
