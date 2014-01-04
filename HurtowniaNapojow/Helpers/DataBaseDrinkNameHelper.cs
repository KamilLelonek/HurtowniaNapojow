using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

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
    }
}