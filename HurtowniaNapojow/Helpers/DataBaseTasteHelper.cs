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
    public static class DataBaseTasteHelper
    {
        private static readonly SmakiTableAdapter TasteTableAdapter = new SmakiTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.SmakiRow> _tastesData = TasteTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.SmakiRow> GetTastesData()
        {
            return _tastesData;
        }

        public static HurtowniaNapojowDataSet.SmakiRow GetTasteByID(int tasteId)
        {
            return GetTastesData().First(taste => taste.Identyfikator == tasteId);
        }

        public static Boolean AddNewTaste(String newTaste)
        {
            var doesTasteExist = GetTastesData().Any(taste => taste.NazwaSmaku == newTaste);
            if (doesTasteExist)
            {
                MessageBox.Show("Smak o podanej nazwie już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            try
            {
                TasteTableAdapter.Insert(newTaste);
                RefreshData();
                MessageBox.Show("Pomyślnie dodano nowy smak", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Wprowadzane dane są nieprawidłowe.\nPole nazwa smaku nie może być puste!", "Błąd");
                return false;
            }
        }

        public static Boolean DeleteTasteRow(DataRow tasteRow)
        {
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_smaku == (tasteRow as HurtowniaNapojowDataSet.SmakiRow).Identyfikator);
            if (productExists)
            {
                MessageBox.Show("Do wybranego smaku '" + (tasteRow as HurtowniaNapojowDataSet.SmakiRow).NazwaSmaku + "' są przypisane napoje producenta. Smak nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            tasteRow.Delete();
            var result = TasteTableAdapter.Update(tasteRow) == 1;
            if (result) RefreshData();
            return result;
        }

        public static Boolean EditTaste(HurtowniaNapojowDataSet.SmakiRow taste, String newTasteName)
        {
            taste.NazwaSmaku = newTasteName;
            return UpdateDB(taste, "Dane smaku zostały zmienione");
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.SmakiRow taste, String messageIfSuccess)
        {
            try
            {
                TasteTableAdapter.Update(taste);
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
            _tastesData = TasteTableAdapter.GetData();
        }
    }
}