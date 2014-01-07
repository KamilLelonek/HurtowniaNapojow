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
            BulkPackageNameTableAdapter.Insert(newBulkPackageName);
            _bulkPackageNameData = BulkPackageNameTableAdapter.GetData();
            MessageBox.Show("Pomyślnie dodano nowy rodzaj opakowania zbiorczego", Globals.TITLE_SUCCESS);
            return true;
        }

        public static Boolean DeleteBulkPackageNameRow(DataRow bulkPackageNameRow)
        {   //tu zmienić na DataBaseBulkPackageHelper 
            var bulkPackageExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_rodzaju_gazu == (bulkPackageNameRow as HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow).Identyfikator);
            if (bulkPackageExists)
            {
                MessageBox.Show("Do wybranej nazwy rodzaju opakowania zbiorczego'" + (bulkPackageNameRow as HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow).NazwaOpakowania + "' są przypisane opakowania zbiorcze. Nazwa opakowania zbiorczego nie zostanie usunięta.", Globals.TITLE_ERROR);
                return false;
            }
            bulkPackageNameRow.Delete();
            return BulkPackageNameTableAdapter.Update(bulkPackageNameRow) == 1;
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
                _bulkPackageNameData = BulkPackageNameTableAdapter.GetData();
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