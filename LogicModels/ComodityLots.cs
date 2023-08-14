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
    public class ComodityPurchase : IPurchase
    {
        public int PurchaseNo { get; set; }
        public string PurchaseTrno { get; set; }
        public DateTime PrchaseDate { get; set; }
        public string PurchaseName { get; set; }//Required for tagging,grouping and UI.
       
     
    }
    public class PurchaseLot
    {
        public int LotId { get; set; }
        public int LotNo { get; set; }//To Extend Application to the lab reports...
        public string LotName { get; set; }
        public string Sauda { get; set; }
        
        //A Transport may have load of different kinds of objects with them..
        //there is an extention:  Different brokers and parties can be seen in reports...
        //While Transportation was single..and they are devided into lots..

        public string PartyName { get; set; }
        public string BrokerName { get; set; }
    }
    public class PurchaseBill
    {
        public int itemId { get; set; }
        public string ItemName { get; set; }
        public decimal SaudaRate { get; set; }
        public decimal BillWeight { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentRate { get; set; }
        public int  Bags { get; set; }
    }

}
