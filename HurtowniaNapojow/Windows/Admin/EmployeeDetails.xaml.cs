using System;
using System.Windows;
using System.Windows.Navigation;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class EmployeeDetails
    {
        private HurtowniaNapojówDataSet.PracownicyRow _employee;
        public EmployeeDetails(ref HurtowniaNapojówDataSet.PracownicyRow employeeRow)
        {
            InitializeComponent();
            _employee = employeeRow;
            EmployeeDetailsGrid.DataContext = employeeRow;
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