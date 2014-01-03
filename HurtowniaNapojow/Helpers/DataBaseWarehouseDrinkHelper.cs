using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseWarehouseDrinkHelper
    {
        private static readonly NapojeHurtowniTableAdapter WarehouseDrinkTableAdapter = new NapojeHurtowniTableAdapter();

        public static IEnumerable<HurtowniaNapojowDataSet.NapojeHurtowniRow> GetWarehouseDrinkData()
        {
            return WarehouseDrinkTableAdapter.GetData();
        }

        public static HurtowniaNapojowDataSet.NapojeHurtowniRow GetDrinkById(int drinkId)
        {
            return GetWarehouseDrinkData().First(drink => drink.Identyfikator == drinkId);
        }
    }
}