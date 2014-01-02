using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseShoppingHelper
    {
        private static readonly ZakupyKlientaTableAdapter ShoppingTableAdapter = new ZakupyKlientaTableAdapter();
        public static IEnumerable<HurtowniaNapojówDataSet.ZakupyKlientaRow> GetShoppingData()
        {
            return ShoppingTableAdapter.GetData();
        }

        public static IEnumerable<HurtowniaNapojówDataSet.ZakupyKlientaRow> GetShoppingForEmployee(HurtowniaNapojówDataSet.PracownicyRow employeeRow)
        {
            var customerShoppingTableData = GetShoppingData();
            return
                from customerShopping
                in customerShoppingTableData
                where customerShopping.id_pracownika == employeeRow.Identyfikator
                select customerShopping;
        }

        public static float CalculateShoppingProfit(HurtowniaNapojówDataSet.ZakupyKlientaRow shopping)
        {
            var customerProducts = DataBaseProductHelper.GetProductsForShopping(shopping);
            return (float)customerProducts.Aggregate(.0, (sum, customerProduct) =>
                sum + DataBaseProductHelper.GetProductProfit(customerProduct));
        }
    }
}