﻿using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProducerDrinkHelper
    {
        private static readonly NapojeProducentaTableAdapter ProducerDrinkTableAdapter = new NapojeProducentaTableAdapter();

        public static IEnumerable<HurtowniaNapojowDataSet.NapojeProducentaRow> GetProducerDrinkData()
        {
            return ProducerDrinkTableAdapter.GetData();
        }

        public static HurtowniaNapojowDataSet.NapojeProducentaRow GetDrinkByID(int drinkId)
        {
            return GetProducerDrinkData().First(drink => drink.Identyfikator == drinkId);
        }
    }
}