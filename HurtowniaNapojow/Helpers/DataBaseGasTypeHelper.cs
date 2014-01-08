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
    public static class DataBaseGasTypeHelper
    {
        private static readonly RodzajeGazuTableAdapter GasTypeTableAdapter = new RodzajeGazuTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.RodzajeGazuRow> _gasTypesData = GasTypeTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.RodzajeGazuRow> GetGasTypeData()
        {
            return _gasTypesData;
        }

        public static HurtowniaNapojowDataSet.RodzajeGazuRow GetGasTypeByID(int gasTypeId)
        {
            return GetGasTypeData().First(gasType => gasType.Identyfikator == gasTypeId);
        }

        public static Boolean AddNewGasType(String newGasType)
        {
            var doesTasteExist = GetGasTypeData().Any(taste => taste.NazwaRodzaju == newGasType);
            if (doesTasteExist)
            {
                MessageBox.Show("Wprowadzany rodzaj gazu już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            
            try
            {
                GasTypeTableAdapter.Insert(newGasType);
                RefreshData();
                MessageBox.Show("Pomyślnie dodano nowy rodzaj gazu", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Wprowadzane dane są nieprawidłowe.\nPole nazwa rodzaju gazu nie może być puste!", "Błąd");
                return false;
            }
        }

        public static Boolean DeleteGasTypeRow(DataRow gasTypeRow)
        {
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_rodzaju_gazu == (gasTypeRow as HurtowniaNapojowDataSet.RodzajeGazuRow).Identyfikator);
            if (productExists)
            {
                MessageBox.Show("Do wybranego rodzaju gazu '" + (gasTypeRow as HurtowniaNapojowDataSet.RodzajeGazuRow).NazwaRodzaju + "' są przypisane napoje producenta. Rodzaj Gazu nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            gasTypeRow.Delete();
            var result = GasTypeTableAdapter.Update(gasTypeRow) == 1;
            if (result) RefreshData();
            return result;
        }

        public static Boolean EditGasType(HurtowniaNapojowDataSet.RodzajeGazuRow taste, String newGasTypeName)
        {
            taste.NazwaRodzaju = newGasTypeName;
            return UpdateDB(taste, "Rodzaj Gazu został zmieniony");
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.RodzajeGazuRow taste, String messageIfSuccess)
        {
            try
            {
                GasTypeTableAdapter.Update(taste);
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
            _gasTypesData = GasTypeTableAdapter.GetData();
        }
    }
}