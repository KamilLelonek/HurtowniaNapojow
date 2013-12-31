using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseEmployeeHelper
    {
        private static readonly PracownicyTableAdapter EmployeesTableAdapter = new PracownicyTableAdapter();
        public static Boolean IsUserAuthenticated(HurtowniaNapojówDataSet.PracownicyRow employee, String password)
        {
            return employee.Hasło.Equals(password);
        }

        public static Boolean ChangePassword(HurtowniaNapojówDataSet.PracownicyRow employee, String newPassword)
        {
            employee.Hasło = newPassword;
            return UpdateDB(employee, "Hasło zostało zmienione");
        }

        public static Boolean ChangeEmail(HurtowniaNapojówDataSet.PracownicyRow employee, String newEmail)
        {
            employee.Email = newEmail;
            return UpdateDB(employee, "Email został zmieniony");
        }

        public static Boolean ChangeName(HurtowniaNapojówDataSet.PracownicyRow employee, String newFirstName, String newLastName)
        {
            employee.Imię = newFirstName;
            employee.Nazwisko = newLastName;
            return UpdateDB(employee, "Dane osobowe zostały zmienione");
        }
        public static Boolean UpdateDB(HurtowniaNapojówDataSet.PracownicyRow employee, String messageIfSuccess)
        {
            try
            {
                EmployeesTableAdapter.Update(employee);
                MessageBox.Show(messageIfSuccess, "Sukces");
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, "Błąd");
                return false;
            }
        }
    }
}
