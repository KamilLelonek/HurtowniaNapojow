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
            TasteTableAdapter.Insert(newTaste);
            _tastesData = TasteTableAdapter.GetData();
            MessageBox.Show("Pomyślnie dodano nowy smak", Globals.TITLE_SUCCESS);
            return true;
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
            return TasteTableAdapter.Update(tasteRow) == 1;
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
                _tastesData = TasteTableAdapter.GetData();
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