using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
///CreatedBY: manishrajsingh@live.in
///CreateAt : 24-07-2023
///HonoredBy : Radiant System Service,STPL@Lucknow
/// </summary>
namespace MPurchase.Models
{
   public class LotPurchase
    {
        public int LotNo { get; set; } = 1; //At least one lot would be there..
        public string SaudaFor { get; set; }//Action:Fetch from DB, Events: OnClick,OnActive

        public string PartyName { get; set; }// Action:Fetch from DB, Events: OnClick,OnActive
        public string BrokerName { get; set; }//Action:Fetch from DB,Prompt for Existing sauda!, Events: Onclick,OnActive,
        public string ChalanNo { get; set; } = "0";
        public string DateChalan { get; set; } = DateTime.Now.ToShortDateString();
        public string BillNo { get; set; } = "0";
        public string DateBill { get; set; } = DateTime.Now.ToShortDateString();
        public string ComodityName { get; set; }
        public int Bags { get; set; }


    }
}
