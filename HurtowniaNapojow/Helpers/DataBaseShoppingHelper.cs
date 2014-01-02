using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseShoppingHelper
    {
        private static readonly ZakupyKlientaTableAdapter ShoppingTableAdapter = new ZakupyKlientaTableAdapter();
        public static IEnumerable<HurtowniaNapojowDataSet.ZakupyKlientaRow> GetShoppingData()
        {
            return ShoppingTableAdapter.GetData();
        }

        public static IEnumerable<HurtowniaNapojowDataSet.ZakupyKlientaRow> GetShoppingForEmployee(HurtowniaNapojowDataSet.PracownicyRow employeeRow)
        {
            var customerShoppingTableData = GetShoppingData();
            return
                from customerShopping
                in customerShoppingTableData
                where customerShopping.id_pracownika == employeeRow.Identyfikator
                select customerShopping;
        }

        public static float CalculateShoppingProfit(HurtowniaNapojowDataSet.ZakupyKlientaRow shopping)
        {
            var customerProducts = DataBaseProductHelper.GetProductsForShopping(shopping);
            return (float)customerProducts.Aggregate(.0, (sum, customerProduct) =>
                sum + DataBaseProductHelper.GetProductProfit(customerProduct));
        }
    }
}