using System;
using System.Linq;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class EmployeeDetailsWindow
    {
        private readonly HurtowniaNapojówDataSet.PracownicyRow _employee;
        private readonly Validator _validator = Validator.Instance;

        public EmployeeDetailsWindow(ref HurtowniaNapojówDataSet.PracownicyRow employeeRow)
        {
            InitializeComponent();
            _employee = employeeRow;
            InitDataContext(employeeRow);
        }

        private void InitDataContext(HurtowniaNapojówDataSet.PracownicyRow employee)
        {
            var customerShoppingTable = DataBaseShoppingHelper.GetShoppingForEmployee(employee);

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

        private void ProfitsButton_Click(object sender, RoutedEventArgs e)
        {
            var profits = DataBaseEmployeeHelper.CalculateEmployeeProfits(_employee);
            MessageBox.Show(String.Format("Bieżące zyski pracownika wynoszą {0}zł", profits));
        }
    }
}