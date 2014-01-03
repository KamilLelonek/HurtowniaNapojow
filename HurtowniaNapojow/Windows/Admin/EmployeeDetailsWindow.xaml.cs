using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Reports;
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
        private readonly IEnumerable<EmployeeShopping> _customerShoppingTable;

        public EmployeeDetailsWindow(ref HurtowniaNapojowDataSet.PracownicyRow employeeRow)
        {
            InitializeComponent();
            _employee = employeeRow;
            _customerShoppingTable = EmployeeShopping.EmployeeShoppingCollectionBuilder(employeeRow);

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
            DataBaseEmployeeHelper.UpdateDB(_employee, "Dane użytkownika zaktualizowane pomyślnie");
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new AdminWindow());
        }

        private void SummaryButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ReportWindow(_customerShoppingTable, @"Admin/EmployeeShoppings.rdlc"), blockPrevious: true);
        }
    }
}