using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProductHelper
    {
        private static readonly ProduktyKlientaTableAdapter ProductsTableAdapter = new ProduktyKlientaTableAdapter();
        public static IEnumerable<HurtowniaNapojowDataSet.ProduktyKlientaRow> GetProductsData()
        {
            return ProductsTableAdapter.GetData();
        }

        public static IEnumerable<HurtowniaNapojowDataSet.ProduktyKlientaRow> GetProductsForShopping(HurtowniaNapojowDataSet.ZakupyKlientaRow shopping)
        {
            var customerProductsDataTable = GetProductsData();
            return
                from customerProduct
                in customerProductsDataTable
                where customerProduct.id_zakupu_klienta == shopping.Identyfikator
                select customerProduct;
        }

        public static float GetProductValue(HurtowniaNapojowDataSet.ProduktyKlientaRow product)
        {
            var warehouseDrink = DataBaseWarehouseDrinkHelper.GetDrinkById(product.id_napoju_hurtowni);
            return DataBaseWarehouseDrinkHelper.CalculateDrinkValue(warehouseDrink);
        }

        internal static object DeleteProductRow(System.Data.DataRow dataRow)
        {
            throw new System.NotImplementedException();
        }
    }
}