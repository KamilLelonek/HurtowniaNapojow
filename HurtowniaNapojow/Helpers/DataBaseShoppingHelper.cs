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
    public static class DataBaseShoppingHelper
    {
        private static readonly ZakupyKlientaTableAdapter ShoppingTableAdapter = new ZakupyKlientaTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.ZakupyKlientaRow> _customersShoppingData = ShoppingTableAdapter.GetData();
        public static IEnumerable<HurtowniaNapojowDataSet.ZakupyKlientaRow> GetShoppingData()
        {
            return _customersShoppingData;
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

        public static HurtowniaNapojowDataSet.ZakupyKlientaRow AddNewShopping(HurtowniaNapojowDataSet.PracownicyRow employeeRow, HurtowniaNapojowDataSet.KlienciRow customerRow)
        {
            ShoppingTableAdapter.Insert(customerRow.Identyfikator, employeeRow.Identyfikator, DateTime.Now);
            _customersShoppingData = ShoppingTableAdapter.GetData();
            return _customersShoppingData.Last();
        }

        public static float CalculateShoppingPrice(HurtowniaNapojowDataSet.ZakupyKlientaRow shopping)
        {
            var customerProducts = DataBaseProductHelper.GetProductsForShopping(shopping);
            return (float)customerProducts.Aggregate(.0, (sum, customerProduct) => sum + customerProduct.Kwota);
        }

        public static int CalculateShoppingProductsTotalAmount(HurtowniaNapojowDataSet.ZakupyKlientaRow shopping)
        {
            var customerProducts = DataBaseProductHelper.GetProductsForShopping(shopping);
            return (int)customerProducts.Aggregate(.0, (sum, customerProduct) => sum + customerProduct.Liczba);
        }

        public static int CalculateShoppingProductsCount(HurtowniaNapojowDataSet.ZakupyKlientaRow shopping)
        {
            var customerProducts = DataBaseProductHelper.GetProductsForShopping(shopping);
            return customerProducts.Count();
        }

        public static Boolean DeleteShoppingRow(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow)
        {
            var productsExists = DataBaseProductHelper.GetProductsForShopping(shoppingRow).Count() > 0;
            if (productsExists)
            {
                MessageBox.Show("Do wybranych zakupów klienta są przypisane produkty klienta. Zakupy nie zostaną usunięte.", Globals.TITLE_ERROR);
                return false;
            }
            shoppingRow.Delete();
            return ShoppingTableAdapter.Update(shoppingRow) == 1;
        }
    }
}