using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProducerDrinkHelper
    {
        private static readonly NapojeProducentaTableAdapter ProducerDrinkTableAdapter = new NapojeProducentaTableAdapter();

        public static HurtowniaNapojówDataSet.NapojeProducentaDataTable GetProducerDrinkData()
        {
            return ProducerDrinkTableAdapter.GetData();
        }

        public static HurtowniaNapojówDataSet.NapojeProducentaRow GetDrinkByID(int drinkId)
        {
            return GetProducerDrinkData().FindByIdentyfikator(drinkId);
        }
    }
}