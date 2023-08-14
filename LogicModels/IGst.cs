using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPurchase.LogicModels
{
    public interface IGst
    {
        decimal CGST_Rate_Percent { get; set; }
        decimal SGST_UTGST_Rate_Persent { get; set; }
        decimal IGST_Rate_Persent { get; set; }
    }
}
