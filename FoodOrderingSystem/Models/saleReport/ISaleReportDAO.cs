using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.saleReport
{
    public interface ISaleReportDAO
    {
        IList<SaleReportObj> ListSaleReport(DateTime fromDate, DateTime toDate);
    }
}
