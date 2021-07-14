using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.saleReport
{
    public class SaleReportDAO : ISaleReportDAO
    {
        public IList<SaleReportObj> ListSaleReport(DateTime fromDate, DateTime toDate)
        {
            return null;
        }
    }
}
