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
    public static class DataBaseDrinkTypeHelper
    {
        private static readonly RodzajeNapojuTableAdapter DrinkTypeTableAdapter = new RodzajeNapojuTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.RodzajeNapojuRow> _drinkTypesData = DrinkTypeTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.RodzajeNapojuRow> GetDrinkTypesData()
        {
            return _drinkTypesData;
        }

        public static HurtowniaNapojowDataSet.RodzajeNapojuRow GetDrinkTypeByID(int drinkTypeId)
        {
            return GetDrinkTypesData().First(drinkTType => drinkTType.Identyfikator == drinkTypeId);
        }

        public static Boolean AddNewDrinkType(String newDrinkType, float newTaxRate )
        {
            var doesDrinkTypeExist = GetDrinkTypesData().Any(drinkType => drinkType.NazwaRodzajuNapoju == newDrinkType);
            if (doesDrinkTypeExist)
            {
                MessageBox.Show("Wprowadzany rodzaj gazu już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            DrinkTypeTableAdapter.Insert(newDrinkType,newTaxRate);
            _drinkTypesData = DrinkTypeTableAdapter.GetData();
            MessageBox.Show("Pomyślnie dodano nowy rodzaj napoju", Globals.TITLE_SUCCESS);
            return true;
        }

        public static Boolean DeleteDrinkTypeRow(DataRow drinkTypeRow)
        {
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_rodzaju_napoju == (drinkTypeRow as HurtowniaNapojowDataSet.RodzajeNapojuRow).Identyfikator);
            if (productExists)
            {
                MessageBox.Show("Do wybranego rodzaju napoju '" + (drinkTypeRow as HurtowniaNapojowDataSet.RodzajeNapojuRow).NazwaRodzajuNapoju + "' są przypisane napoje producenta. Rodzaj napoju nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            drinkTypeRow.Delete();
            return DrinkTypeTableAdapter.Update(drinkTypeRow) == 1;
        }

        public static Boolean EditDrinkType(HurtowniaNapojowDataSet.RodzajeNapojuRow drinkType, String newDrinkTypeName, float newTaxRate )
        {
            drinkType.NazwaRodzajuNapoju = newDrinkTypeName;
            drinkType.StawkaPodatkowa = newTaxRate;
            return UpdateDB(drinkType, "Rodzaj napoju został zmieniony");

        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.RodzajeNapojuRow drinkType, String messageIfSuccess)
        {
            try
            {
                DrinkTypeTableAdapter.Update(drinkType);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                _drinkTypesData = DrinkTypeTableAdapter.GetData();
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