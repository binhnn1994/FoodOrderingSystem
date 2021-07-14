using FoodOrderingSystem.Models.saleReport;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class SaleReportService : ISaleReportService
    {
        private ISaleReportDAO _saleReportDAO;
        public SaleReportService(ISaleReportDAO saleReportDAO) => _saleReportDAO = saleReportDAO;
        public IList<SaleReportObj> ListSaleReport(DateTime fromDate, DateTime toDate)
            => _saleReportDAO.ListSaleReport(fromDate, toDate);
    }
}
