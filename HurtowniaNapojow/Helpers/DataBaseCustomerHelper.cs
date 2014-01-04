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
    public static class DataBaseCustomerHelper
    {
        private static readonly KlienciTableAdapter CustomerTableAdapter = new KlienciTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.KlienciRow> _customersData = CustomerTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.KlienciRow> GetCustomersData()
        {
            return _customersData;
        }

        public static HurtowniaNapojowDataSet.KlienciRow GetCustomerForShopping(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow)
        {
            return GetCustomersData().First(customer => customer.Identyfikator == shoppingRow.id_klienta);
        }

        public static Boolean AddNewCustomer(String newNazwaKlienta, String newNIP, String newNrTelefonu, String newEmail, String newUlicaNr, String newMiastoKod)
        {
            var doesCustomerExist = GetCustomersData().Any(customer => customer.NazwaKlienta == newNazwaKlienta);
            if (doesCustomerExist)
            {
                MessageBox.Show("Klient o podanej nazwie już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            CustomerTableAdapter.Insert(newNazwaKlienta, newNIP, newNrTelefonu, newEmail, newUlicaNr, newMiastoKod);
            _customersData = CustomerTableAdapter.GetData();
            MessageBox.Show("Pomyślnie dodano nowego klienta", Globals.TITLE_SUCCESS);
            return true;
        }

        public static Boolean DeleteCustomerRow(DataRow customerRow)
        {
            var shoppingExists = DataBaseShoppingHelper.GetShoppingData().Any(shopping => shopping.id_klienta == (customerRow as HurtowniaNapojowDataSet.KlienciRow).Identyfikator);
            if (shoppingExists)
            {
                MessageBox.Show("Do wybranego klienta '" + (customerRow as HurtowniaNapojowDataSet.KlienciRow).NazwaKlienta + "' są przypisane zakupy klienta. Klient nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            customerRow.Delete();
            return CustomerTableAdapter.Update(customerRow) == 1;
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.KlienciRow customer, String messageIfSuccess)
        {
            try
            {
                CustomerTableAdapter.Update(customer);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
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