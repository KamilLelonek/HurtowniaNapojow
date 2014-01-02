using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    class DataBaseCustomerHelper
    {
        private static readonly KlienciTableAdapter CustomerTableAdapter = new KlienciTableAdapter();

        public static HurtowniaNapojówDataSet.KlienciDataTable GetCustomersData()
        {
            return CustomerTableAdapter.GetData();
        }

        public static HurtowniaNapojówDataSet.KlienciRow GetCustomerForShopping(HurtowniaNapojówDataSet.ZakupyKlientaRow shoppingRow)
        {
            return GetCustomersData().FindByIdentyfikator(shoppingRow.id_klienta);
        }
    }
}