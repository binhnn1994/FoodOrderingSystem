using FoodOrderingSystem.Models.saleReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface ISaleReportService
    {
        IList<SaleReportObj> ListSaleReport(DateTime fromDate, DateTime toDate);
    }
}
