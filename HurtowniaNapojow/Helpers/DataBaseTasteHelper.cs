using System.Collections.Generic;
using System.Linq;
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
    }
}