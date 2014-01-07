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
    public static class DataBasePiecePackageNameHelper
    {
        private static readonly NazwyOpakowaniaSztukiTableAdapter PiecePackageNameTableAdapter = new NazwyOpakowaniaSztukiTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow> _piecePackageNameData = PiecePackageNameTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow> GetPiecePackageNameData()
        {
            return _piecePackageNameData;
        }

        public static HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow GetPiecePackageNameByID(int piecePackageNameId)
        {
            return GetPiecePackageNameData().First(piecePackageName => piecePackageName.Identyfikator == piecePackageNameId);
        }

        public static Boolean AddNewPiecePackageName(String newPiecePackageName)
        {
            var doesPiecePackageNameExist = GetPiecePackageNameData().Any(piecePackageName => piecePackageName.NazwaOpakowania == newPiecePackageName);
            if (doesPiecePackageNameExist)
            {
                MessageBox.Show("Wprowadzana nazwa rodzaju opakowania sztuki już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            PiecePackageNameTableAdapter.Insert(newPiecePackageName);
            _piecePackageNameData = PiecePackageNameTableAdapter.GetData();
            MessageBox.Show("Pomyślnie dodano nową nazwę rodzaju opakowania sztuki", Globals.TITLE_SUCCESS);
            return true;
        }

        public static Boolean DeletePiecePackageNameRow(DataRow piecePackageNameRow)
        {   //tu zmienić na DataBasePiecePackageHelper 
            var piecePackageExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_rodzaju_gazu == (piecePackageNameRow as HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow).Identyfikator);
            if (piecePackageExists)
            {
                MessageBox.Show("Do wybranej nazwy rodzaju opakowania sztuki'" + (piecePackageNameRow as HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow).NazwaOpakowania + "' są przypisane opakowania sztuki. Nazwa rodzaju opakowania sztuki nie zostanie usunięta.", Globals.TITLE_ERROR);
                return false;
            }
            piecePackageNameRow.Delete();
            return PiecePackageNameTableAdapter.Update(piecePackageNameRow) == 1;
        }

        public static Boolean EditPiecePackageName(HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow piecePackageName, String newPiecePackageName)
        {
            piecePackageName.NazwaOpakowania = newPiecePackageName;
            return UpdateDB(piecePackageName, "Nazwa rodzaju opakowania sztuki została zmieniona");

        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow piecePackageName, String messageIfSuccess)
        {
            try
            {
                PiecePackageNameTableAdapter.Update(piecePackageName);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                _piecePackageNameData = PiecePackageNameTableAdapter.GetData();
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