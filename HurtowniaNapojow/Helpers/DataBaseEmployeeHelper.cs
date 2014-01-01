using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseEmployeeHelper
    {
        private static readonly PracownicyTableAdapter EmployeesTableAdapter = new PracownicyTableAdapter();

        public static HurtowniaNapojówDataSet.PracownicyDataTable GetEmployeesData()
        {
            return EmployeesTableAdapter.GetData();
        }

        #region Basic CRUD
        public static Boolean AddNewEmployee(String newFirstName, String newLastName, String newEmail, Boolean hasAdminRole)
        {
            var doesEmployeeExist = GetEmployeesData().Any(employee => employee.Email == newEmail);
            if (doesEmployeeExist)
            {
                MessageBox.Show("Pracownik o podanym adresie email już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            EmployeesTableAdapter.Insert(newLastName, newFirstName, Globals.DEFAULT_PASSWORD, newEmail, hasAdminRole);
            MessageBox.Show("Pomyślnie dodano nowego pracownika", Globals.TITLE_SUCCESS);
            return true;
        }

        public static Boolean DeleteEmployeeRow(DataRow employeeRow)
        {
            employeeRow.Delete();
            return EmployeesTableAdapter.Update(employeeRow) == 1;
        }

        #endregion Basic CRUD

        #region Extended CRUD
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

        public static Boolean ChangeName(HurtowniaNapojówDataSet.PracownicyRow employee, String newFirstName,
            String newLastName)
        {
            employee.Imię = newFirstName;
            employee.Nazwisko = newLastName;
            return UpdateDB(employee, "Dane osobowe zostały zmienione");
        }

        public static float CalculateEmployeeProfits(HurtowniaNapojówDataSet.PracownicyRow employee)
        {
            var employeeShoppings = DataBaseShoppingHelper.GetShoppingForEmployee(employee);
            return (float)employeeShoppings.Aggregate(.0, (sum, profit) => sum + DataBaseShoppingHelper.CalculateShoppingProfit(profit));
        }
        #endregion Extended CRUD

        #region Common methods
        public static Boolean UpdateDB(HurtowniaNapojówDataSet.PracownicyRow employee, String messageIfSuccess)
        {
            try
            {
                EmployeesTableAdapter.Update(employee);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                return false;
            }
        }
        #endregion Common methods
    }
}