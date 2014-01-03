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

        public static IEnumerable<HurtowniaNapojowDataSet.ZakupyKlientaRow> GetShoppingForCustomer(HurtowniaNapojowDataSet.KlienciRow customerRow)
        {
            var customerShoppingTableData = GetShoppingData();
            return
                from customerShopping
                in customerShoppingTableData
                where customerShopping.id_klienta == customerRow.Identyfikator
                select customerShopping;
        }

        public static float CalculateShoppingProfit(HurtowniaNapojowDataSet.ZakupyKlientaRow shopping)
        {
            var customerProducts = DataBaseProductHelper.GetProductsForShopping(shopping);
            return (float)customerProducts.Aggregate(.0, (sum, customerProduct) =>
                sum + DataBaseProductHelper.GetProductProfit(customerProduct));
        }

        public static float CalculateShoppingPrice(HurtowniaNapojowDataSet.ZakupyKlientaRow shopping)
        {
            var customerProducts = DataBaseProductHelper.GetProductsForShopping(shopping);
            return (float)customerProducts.Aggregate(.0, (sum, customerProduct) =>
                sum + customerProduct.Kwota);
        }

        internal static object DeleteShoppingRow(System.Data.DataRow dataRow)
        {
            throw new System.NotImplementedException();
        }
    }
}