using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProductHelper
    {
        private static readonly ProduktyKlientaTableAdapter ProductsTableAdapter = new ProduktyKlientaTableAdapter();
        public static HurtowniaNapojówDataSet.ProduktyKlientaDataTable GetProductsData()
        {
            return ProductsTableAdapter.GetData();
        }

        public static IEnumerable<HurtowniaNapojówDataSet.ProduktyKlientaRow> GetProductsForShopping(HurtowniaNapojówDataSet.ZakupyKlientaRow shopping)
        {
            var customerProductsDataTable = GetProductsData();
            return
                from customerProduct
                in customerProductsDataTable
                where customerProduct.id_zakupu_klienta == shopping.Identyfikator
                select customerProduct;
        }

        public static float GetProductProfit(HurtowniaNapojówDataSet.ProduktyKlientaRow product)
        {
            var warehouseDrink = DataBaseWarehouseDrinkHelper.GetDrinkByID(product.id_napoju_hurtowni);
            return DataBaseWarehouseDrinkHelper.CalculateDrinkProfit(warehouseDrink);
        }
    }
}