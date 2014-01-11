using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using System.Windows;
using System;
using System.Data.OleDb;
using System.Data;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseWarehouseDrinkHelper
    {
        private static readonly NapojeHurtowniTableAdapter WarehouseDrinkTableAdapter = new NapojeHurtowniTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.NapojeHurtowniRow> _warehouseDrinksData = WarehouseDrinkTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.NapojeHurtowniRow> GetWarehouseDrinkData()
        {
            return _warehouseDrinksData;
        }

        public static HurtowniaNapojowDataSet.NapojeHurtowniRow GetDrinkById(int drinkId)
        {
            return GetWarehouseDrinkData().First(drink => drink.Identyfikator == drinkId);
        }

        public static Boolean AddNewWarehouseDrink(int idDrink, int quantity, float warehousePrice, string location, DateTime expirationDate)
        {
            var doesWarehouseDrinkExist = GetWarehouseDrinkData().Any(warehouseDrink => (warehouseDrink.id_napoju_producenta == idDrink) && (warehouseDrink.LiczbaSztuk == quantity) && (warehouseDrink.CenaHurtowni == warehousePrice)
                                                                                            && (warehouseDrink.Położenie == location) && (warehouseDrink.DataWażności == expirationDate));
            if (doesWarehouseDrinkExist)
            {
                MessageBox.Show("Wprowadzana partia towaru już istnieje w bazie magazynu hurtowni", Globals.TITLE_ERROR);
                return false;
            }
            try
            {
                WarehouseDrinkTableAdapter.Insert(idDrink, quantity, warehousePrice, location, expirationDate);
                RefreshData();
                MessageBox.Show("Pomyślnie dodano partię towarów", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Wprowadzane dane są nieprawidłowe.\n!", "Błąd");
                return false;
            }
        }

        public static Boolean DeleteWarehouseDrinkRow(DataRow warehouseDrinkRow)
        {
            var warehouseProductExists = DataBaseProductHelper.GetProductsData().Any(product => product.id_napoju_hurtowni == (warehouseDrinkRow as HurtowniaNapojowDataSet.NapojeHurtowniRow).Identyfikator);
            if (warehouseProductExists)
            {
                MessageBox.Show("Do wybranego napoju z magazynu są przypisane produkty zakópów klienta. Wybrana pozycja magazynu nie zostanie usunięta.", Globals.TITLE_ERROR);
                return false;
            }
            warehouseDrinkRow.Delete();
            var result = WarehouseDrinkTableAdapter.Update(warehouseDrinkRow) == 1;
            if (result) RefreshData();
            return result;
        }

        public static Boolean EditWarehouseDrink(HurtowniaNapojowDataSet.NapojeHurtowniRow warehouseDrink, int idDrink, int quantity, float warehousePrice, string location, DateTime expirationDate)
        {
            warehouseDrink.id_napoju_producenta = idDrink;
            warehouseDrink.LiczbaSztuk = quantity;
            warehouseDrink.CenaHurtowni = warehousePrice;
            warehouseDrink.Położenie = location;
            warehouseDrink.DataWażności = expirationDate;
            

            return UpdateDB(warehouseDrink, "Napój z magazynu hurtowni został zmieniony");
        }

        public static Boolean UpdateAmount(HurtowniaNapojowDataSet.ProduktyKlientaRow product, int amountDelta)
        {
            HurtowniaNapojowDataSet.NapojeHurtowniRow warehouseDrink = GetDrinkById(product.id_napoju_hurtowni);
            if (warehouseDrink.LiczbaSztuk >= amountDelta)
            {
                warehouseDrink.LiczbaSztuk -= amountDelta;
                return UpdateDB(warehouseDrink, "Napój z magazynu hurtowni został zmieniony", false);
            }
            else
            {
                MessageBox.Show("Nie ma wystarczającej liczby sztuk w magazynie. Pozostała dostępna liczba sztuk produktu: " + warehouseDrink.LiczbaSztuk);
                return false;
            }
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.NapojeHurtowniRow warehouseDrink, String messageIfSuccess, bool showMessageIfSuccess = true)
        {
            try
            {
                WarehouseDrinkTableAdapter.Update(warehouseDrink);
                if (showMessageIfSuccess)
                    MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                RefreshData();
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Edycja danych nie może być przeprowadzona ponieważ w bazie danych istnieje rekord zawierający wprowadzane dane lub wprowadzane dane są nieprawidłowe.\nPole nie może być puste!", "Błąd");
                return false;
            }
        }

        private static void RefreshData()
        {
            _warehouseDrinksData = WarehouseDrinkTableAdapter.GetData();
        }

        public static IEnumerable<WarehouseDrink> GetWarehouseDrinkWithZeroQuantity()
        {
            return from warehouseDrink in _warehouseDrinksData
                       where warehouseDrink.LiczbaSztuk == 0
                       select new WarehouseDrink(warehouseDrink);
        }
    }
}