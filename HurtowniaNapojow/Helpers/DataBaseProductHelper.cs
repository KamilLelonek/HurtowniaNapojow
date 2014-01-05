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

        public static HurtowniaNapojowDataSet.ProduktyKlientaRow AddNewProduct(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow, HurtowniaNapojowDataSet.NapojeHurtowniRow drinkRow)
        {
            ProductsTableAdapter.Insert(shoppingRow.Identyfikator, drinkRow.Identyfikator, 0, 0);
            _productsData = ProductsTableAdapter.GetData();
            return _productsData.First(p => p.id_zakupu_klienta == shoppingRow.Identyfikator && p.id_napoju_hurtowni == drinkRow.Identyfikator && p.Kwota == 0 && p.Liczba == 0);
        }

        public static Boolean DeleteProductRow(HurtowniaNapojowDataSet.ProduktyKlientaRow productRow)
        {
            productRow.Delete();
            return ProductsTableAdapter.Update(productRow) == 1;
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.ProduktyKlientaRow productRow, String messageIfSuccess)
        {
            try
            {
                ProductsTableAdapter.Update(productRow);
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                return false;
            }
        }
    }
}