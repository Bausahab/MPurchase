using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPurchase.LogicModels
{
    public class PurchaseHead
    {
        public int LastPurEntryNO { get; private set; } = 0;
        public int TrNumber { get; private set; } = 0;
        public int GrNo { get; private set; } = 0;
        public DateTime LastPurDate { get; private set; }
        public string OrgACfrt { get; private set; }
        public string TransprtDtl { get; private set; }

        public PurchaseHead(int lastpurchseentryno, int transitionno, int grno, DateTime lastpurchasedate, string orgacfrt, string transportdetails)
        {
            this.LastPurEntryNO = lastpurchseentryno;
            this.TrNumber = transitionno;
            this.GrNo = grno;
            this.LastPurEntryNO = lastpurchseentryno;
            this.OrgACfrt = orgacfrt;
            this.TransprtDtl = transportdetails;
        }
        // public PurchaseHead LastPurchses { get; set; }

    }
}
