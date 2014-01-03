using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProductHelper
    {
        private static readonly ProduktyKlientaTableAdapter ProductsTableAdapter = new ProduktyKlientaTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.ProduktyKlientaRow> _productsData = ProductsTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.ProduktyKlientaRow> GetProductsData()
        {
            return _productsData;
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

        internal static object DeleteProductRow(System.Data.DataRow dataRow)
        {
            throw new System.NotImplementedException();
        }
    }
}