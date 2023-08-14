using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPurchase.LogicModels
{
    public class ComodityLots : IOrder
    {
        //fetch como
        // if >1
        //list como
        //add to list
        // save to mpur
        // 
        public int Orderid { get; set; }
        public string LotId { get; set; }
        public string LotName { get; set; }
        public string MyProperty { get; set; }

    }
}
