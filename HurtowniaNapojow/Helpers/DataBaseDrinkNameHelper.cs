using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using System;
using System.Windows;
using System.Data;
using System.Data.OleDb;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseDrinkNameHelper
    {
        private static readonly NazwyNapojuTableAdapter DrinkNameTableAdapter = new NazwyNapojuTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.NazwyNapojuRow> _drinkNamesData = DrinkNameTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.NazwyNapojuRow> GetDrinkNamesData()
        {
            return _drinkNamesData;
        }

        public static HurtowniaNapojowDataSet.NazwyNapojuRow GetDrinkNameByID(int drinkNameId)
        {
            return GetDrinkNamesData().First(drinkName => drinkName.Identyfikator == drinkNameId);
        }

        public static Boolean AddNewDrinkName(String newDrinkName)
        {
            var doesDrinkNameExist = GetDrinkNamesData().Any(drink => drink.NazwaNapoju == newDrinkName);
            if (doesDrinkNameExist)
            {
                MessageBox.Show("Wprowadzana nazwa napoju już istnieje", Globals.TITLE_ERROR);
                return false;
            }

            try
            {
                DrinkNameTableAdapter.Insert(newDrinkName);
                _drinkNamesData = DrinkNameTableAdapter.GetData();
                MessageBox.Show("Pomyślnie dodano nową nazwę napoju", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                return false;
            }
                
        }

        public static Boolean DeleteDrinkNameRow(DataRow drinkNameRow)
        {
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_nazwy_napoju == (drinkNameRow as HurtowniaNapojowDataSet.NazwyNapojuRow).Identyfikator);
            if (productExists)
            {
                MessageBox.Show("Do wybranej nazwy napoju '" + (drinkNameRow as HurtowniaNapojowDataSet.NazwyNapojuRow).NazwaNapoju + "' są przypisane napoje producenta. Nazwa napoju nie zostanie usunięta.", Globals.TITLE_ERROR);
                return false;
            }
            drinkNameRow.Delete();
            return DrinkNameTableAdapter.Update(drinkNameRow) == 1;
        }

        public static Boolean EditDrinkName(HurtowniaNapojowDataSet.NazwyNapojuRow drinkName, String newDrinkName)
        {
            drinkName.NazwaNapoju = newDrinkName;
            return UpdateDB(drinkName, "Nazwa napoju została zmieniona");

        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.NazwyNapojuRow drinkName, String messageIfSuccess)
        {
            try
            {
                DrinkNameTableAdapter.Update(drinkName);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                _drinkNamesData = DrinkNameTableAdapter.GetData();
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