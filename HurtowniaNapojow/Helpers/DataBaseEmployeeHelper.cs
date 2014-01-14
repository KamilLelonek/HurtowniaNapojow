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
    public static class DataBaseEmployeeHelper
    {
        private static readonly PracownicyTableAdapter EmployeesTableAdapter = new PracownicyTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.PracownicyRow> _employeesData = EmployeesTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.PracownicyRow> GetEmployeesData()
        {
            return _employeesData;
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
            _employeesData = EmployeesTableAdapter.GetData();
            MessageBox.Show(String.Format("Pomyślnie dodano nowego pracownika z domyślnym hasłem {0}", Globals.DEFAULT_PASSWORD), Globals.TITLE_SUCCESS);
            return true;
        }

        public static Boolean DeleteEmployeeRow(HurtowniaNapojowDataSet.PracownicyRow employeeRow)
        {
            if (employeeRow == SessionHelper.Instance.CurrentEmployee)
            {
                MessageBox.Show("Nie można usunąć zalogowanego pracownika.", Globals.TITLE_ERROR);
                return false;
            }
            if (employeeRow.CzyAdministrator)
            {
                MessageBox.Show("Nie można usunąć konta administratora.", Globals.TITLE_ERROR);
                return false;
            }

            employeeRow.Delete();
            try
            {
                return EmployeesTableAdapter.Update(employeeRow) == 1;
            }
            catch (Exception)
            {
                employeeRow.RejectChanges();
                MessageBox.Show("Nie można usunąć pracownika, który realizował zakupy klientów.", Globals.TITLE_ERROR);
                return false;
            }
        }

        #endregion Basic CRUD

        #region Extended CRUD
        public static Boolean IsUserAuthenticated(HurtowniaNapojowDataSet.PracownicyRow employee, String password)
        {
            return employee.Hasło.Equals(password);
        }

        public static Boolean ChangePassword(HurtowniaNapojowDataSet.PracownicyRow employee, String newPassword)
        {
            employee.Hasło = newPassword;
            return UpdateDB(employee, "Hasło zostało zmienione");
        }

        public static Boolean ChangeEmail(HurtowniaNapojowDataSet.PracownicyRow employee, String newEmail)
        {
            employee.Email = newEmail;
            return UpdateDB(employee, "Email został zmieniony");
        }

        public static Boolean ChangeName(HurtowniaNapojowDataSet.PracownicyRow employee, String newFirstName,
            String newLastName)
        {
            employee.Imię = newFirstName;
            employee.Nazwisko = newLastName;
            return UpdateDB(employee, "Dane osobowe zostały zmienione");
        }

        public static float CalculateEmployeeIncome(HurtowniaNapojowDataSet.PracownicyRow employee)
        {
            var employeeShoppings = DataBaseShoppingHelper.GetShoppingForEmployee(employee);
            return (float)employeeShoppings.Aggregate(.0, (sum, income) => sum + DataBaseShoppingHelper.CalculateShoppingPrice(income));
        }
        #endregion Extended CRUD

        public static double CalculateEmployeeProfits(HurtowniaNapojowDataSet.PracownicyRow employee)
        {
            var employeeShoppings = DataBaseShoppingHelper.GetShoppingForEmployee(employee);
            return employeeShoppings.Aggregate(.0, (sum, profit) => sum + DataBaseShoppingHelper.CalculateShoppingProfit(profit));
        }

        #region Common methods
        public static Boolean UpdateDB(HurtowniaNapojowDataSet.PracownicyRow employee, String messageIfSuccess)
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