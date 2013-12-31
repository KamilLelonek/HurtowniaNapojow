using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class EmployeeDetails
    {
        private readonly HurtowniaNapojówDataSet.PracownicyRow _employee;
        public EmployeeDetails(ref HurtowniaNapojówDataSet.PracownicyRow employeeRow)
        {
            InitializeComponent();
            _employee = employeeRow;
            EmployeeDetailsGrid.DataContext = employeeRow;

            var zakupyKlientaTableData = new ZakupyKlientaTableAdapter().GetData();
            var zakupyKlientaTable = from zakupKlienta
                                     in zakupyKlientaTableData
                                     where zakupKlienta.id_pracownika == _employee.Identyfikator
                                     select zakupKlienta;

            EmployeeShoppingDataGrid.DataContext = zakupyKlientaTable.AsEnumerable();
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseEmployeeHelper.ChangePassword(_employee, "password");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseEmployeeHelper.UpdateDB(_employee, "Dane użytkownika zaktualizowane pomyślnie");
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new AdminWindow());
        }
    }
}