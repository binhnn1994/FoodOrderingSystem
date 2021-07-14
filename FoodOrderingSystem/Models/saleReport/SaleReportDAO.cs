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
            var saleReportObj = new List<SaleReportObj>();
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "SELECT  I.itemID, I.itemName, I.categoryName, I.unitPrice, SUM(OD.quantity) as totalQuantity, SUM(OD.quantity*I.unitPrice) as totalSales " +
                                    "FROM customerOrder O, account C, orderDetails OD, item I " +
                                    "WHERE O.customerID = C.userID and O.orderID = OD.orderID and OD.itemID = I.itemID and O.orderDate >= @fromDate and O.orderDate <= @toDate " +
                                    "GROUP BY I.itemID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@fromDate", fromDate);
                        command.Parameters.AddWithValue("@toDate", toDate);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                saleReportObj.Add(new SaleReportObj
                                {
                                    ItemID = reader.GetString(0),
                                    ItemName = reader.GetString(1),
                                    CategoryName = reader.GetString(2),
                                    UnitPrice = reader.GetFloat(3),
                                    TotleQuantity = reader.GetInt32(4),
                                    TotalSales = reader.GetFloat(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return saleReportObj;
        }
    }
}