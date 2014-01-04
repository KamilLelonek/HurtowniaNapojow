using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
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

        public static Boolean DeleteProductRow(HurtowniaNapojowDataSet.ProduktyKlientaRow productRow)
        {
            productRow.Delete();
            return ProductsTableAdapter.Update(productRow) == 1;
        }
    }
}