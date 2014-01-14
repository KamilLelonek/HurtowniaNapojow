using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Reports.Admin;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class EmployeeDetailsWindow : IRebindlable
    {
        private readonly HurtowniaNapojowDataSet.PracownicyRow _employee;
        private readonly Validator _validator = Validator.Instance;
        private IEnumerable<EmployeeShopping> _customerShoppingTable;

        public EmployeeDetailsWindow(ref HurtowniaNapojowDataSet.PracownicyRow employeeRow)
        {
            InitializeComponent();
            _employee = employeeRow;
            _customerShoppingTable = EmployeeShopping.EmployeeShoppingCollectionBuilder(employeeRow);

            EmployeeShoppingDataGrid.FontSize = Globals.DATAGRID_FONT_SIZE;
            InitDataContext(employeeRow);
        }

        private void InitDataContext(HurtowniaNapojowDataSet.PracownicyRow employee)
        {
            EmployeeShoppingDataGrid.DataContext = _customerShoppingTable;
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
            _employee.Imię = FirstNameTextBox.Text;
            _employee.Nazwisko = LastNameTextBox.Text;
            _employee.Email = EmailTextBox.Text;
            DataBaseEmployeeHelper.UpdateDB(_employee, "Dane użytkownika zaktualizowane pomyślnie");
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new AdminWindow());
        }

        private void SummaryButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenReport(_customerShoppingTable, @"Admin/EmployeeShoppings.rdlc");
        }

        private void ProfitsButton_Click(object sender, RoutedEventArgs e)
        {
            var profit = DataBaseEmployeeHelper.CalculateEmployeeProfits(_employee);
            var profitFormatted = Math.Round(profit, 2);
            MessageBox.Show(String.Format("Zyski pracownika dla hurtowni wynoszą {0} zł", profitFormatted), "Zyski");
        }

        private void ShowShoppingDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var dataContext = button.DataContext;
            var employeeShopping = (EmployeeShopping)dataContext;
            this.OpenWindow(new CustomerShoppingDetailsWindow(this, employeeShopping), false, true);
        }

        public void RebindData()
        {
            EmployeeShoppingDataGrid.DataContext = null;
            _customerShoppingTable = EmployeeShopping.EmployeeShoppingCollectionBuilder(_employee);
            EmployeeShoppingDataGrid.DataContext = _customerShoppingTable;
        }
    }
}