using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPurchase.LogicModels
{
    public interface IPurchase
    {
        int PurchaseNo { get; set; }
        string PurchaseTrno { get; set; }//int value may bind application capability.
        DateTime PrchaseDate { get; set; }

    }
}
