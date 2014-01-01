using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseWarehouseDrinkHelper
    {
        private static readonly NapojeHurtowniTableAdapter WarehouseDrinkTableAdapter = new NapojeHurtowniTableAdapter();

        public static HurtowniaNapojówDataSet.NapojeHurtowniDataTable GetWarehouseDrinkData()
        {
            return WarehouseDrinkTableAdapter.GetData();
        }

        public static HurtowniaNapojówDataSet.NapojeHurtowniRow GetDrinkByID(int drinkId)
        {
            return GetWarehouseDrinkData().FindByIdentyfikator(drinkId);
        }

        public static float CalculateDrinkProfit(HurtowniaNapojówDataSet.NapojeHurtowniRow warehouseDrink)
        {
            var producerDrink = DataBaseProducerDrinkHelper.GetDrinkByID(warehouseDrink.id_napoju_producenta);
            return warehouseDrink.CenaHurtowni - producerDrink.CenaProducenta;
        }
    }
}