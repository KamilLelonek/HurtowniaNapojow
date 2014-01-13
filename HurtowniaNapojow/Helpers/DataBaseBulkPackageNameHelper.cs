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
    public static class DataBaseBulkPackageNameHelper
    {
        private static readonly NazwyOpakowaniaZbiorczegoTableAdapter BulkPackageNameTableAdapter = new NazwyOpakowaniaZbiorczegoTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow> _bulkPackageNameData = BulkPackageNameTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow> GetBulkPackageNameData()
        {
            return _bulkPackageNameData;
        }

        public static HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow GetBulkPackageNameByID(int bulkPackageNameId)
        {
            return GetBulkPackageNameData().First(bulkPackageName => bulkPackageName.Identyfikator == bulkPackageNameId);
        }

        public static Boolean AddNewBulkPackageName(String newBulkPackageName)
        {
            var doesBulkPackageNameExist = GetBulkPackageNameData().Any(bulkPackageName => bulkPackageName.NazwaOpakowania == newBulkPackageName);
            if (doesBulkPackageNameExist)
            {
                MessageBox.Show("Wprowadzany rodzaj opakowania zbiorczego już istnieje", Globals.TITLE_ERROR);
                return false;
            }
           
            try
            {
                BulkPackageNameTableAdapter.Insert(newBulkPackageName);
                RefreshData();
                MessageBox.Show("Pomyślnie dodano nowy rodzaj opakowania zbiorczego", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Wprowadzane dane są nieprawidłowe.\nPole nazwa opakowania zbiorczego nie może być puste!", "Błąd");
                return false;
            }
        }

        public static Boolean DeleteBulkPackageNameRow(DataRow bulkPackageNameRow)
        {
            var bulkPackageExists = DataBaseBulkPackageHelper.GetBulkPackageData().Any(bulk => bulk.id_rodzaju_opakowania_zbiorczego == (bulkPackageNameRow as HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow).Identyfikator);
            if (bulkPackageExists)
            {
               MessageBox.Show("Do wybranej nazwy rodzaju opakowania zbiorczego '" + (bulkPackageNameRow as HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow).NazwaOpakowania + "' są przypisane opakowania zbiorcze. Nazwa opakowania zbiorczego nie zostanie usunięta.", Globals.TITLE_ERROR);
               return false;
            }
            try
                {
                    bulkPackageNameRow.Delete();
                    var result = BulkPackageNameTableAdapter.Update(bulkPackageNameRow) == 1;
                    if (result) RefreshData();
                    return result;
                }
                catch (OleDbException e)
                {
                    MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                    return false;
                }
        }

        public static Boolean EditBulkPackageName(HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow bulkPackageName, String newbulkPackageName)
        {
            bulkPackageName.NazwaOpakowania = newbulkPackageName;
            return UpdateDB(bulkPackageName, "Nazwa rodzaju opakowania zbiorczego została zmieniona");
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow bulkPackageName, String messageIfSuccess)
        {
            try
            {
                BulkPackageNameTableAdapter.Update(bulkPackageName);
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
            _bulkPackageNameData = BulkPackageNameTableAdapter.GetData();
        }

    }
}