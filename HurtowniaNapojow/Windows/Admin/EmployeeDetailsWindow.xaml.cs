using System;
using System.Linq;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class EmployeeDetailsWindow
    {
        private readonly HurtowniaNapojowDataSet.PracownicyRow _employee;
        private readonly Validator _validator = Validator.Instance;

        public EmployeeDetailsWindow(ref HurtowniaNapojowDataSet.PracownicyRow employeeRow)
        {
            InitializeComponent();
            _employee = employeeRow;
            InitDataContext(employeeRow);
        }

        private void InitDataContext(HurtowniaNapojowDataSet.PracownicyRow employee)
        {
            var customerShoppingTable = EmployeeShopping.EmployeeShoppingCollectionBuilder(employee);

            EmployeeShoppingDataGrid.DataContext = customerShoppingTable;
            EmployeeDetailsGrid.DataContext = employee;
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseEmployeeHelper.ChangePassword(_employee, Globals.DEFAULT_PASSWORD);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(FirstNameTextBox, LastNameTextBox, EmailTextBox) ||
                !_validator.IsEmailValid(EmailTextBox)) return;
            DataBaseEmployeeHelper.UpdateDB(_employee, "Dane użytkownika zaktualizowane pomyślnie");
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new AdminWindow());
        }

        private void ValueButton_Click(object sender, RoutedEventArgs e)
        {
            var income = DataBaseEmployeeHelper.CalculateEmployeeIncome(_employee);
            MessageBox.Show(String.Format("Bieżące kwota zrealizowanych zakupów wynosi {0}zł", income));
        }
    }
}