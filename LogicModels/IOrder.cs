using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPurchase.LogicModels
{
    public interface IOrder
    {
        int Orderid { get; set; }
        string LotId { get; set; }

    }
}
