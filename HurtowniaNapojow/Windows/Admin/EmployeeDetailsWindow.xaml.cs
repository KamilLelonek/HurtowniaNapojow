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

        private void InitDataContext(HurtowniaNapojówDataSet.PracownicyRow employeeRow)
        {
            var zakupyKlientaTableData = new ZakupyKlientaTableAdapter().GetData();
            var zakupyKlientaTable =
                from zakupKlienta
                    in zakupyKlientaTableData
                where zakupKlienta.id_pracownika == _employee.Identyfikator
                select zakupKlienta;

            EmployeeShoppingDataGrid.DataContext = zakupyKlientaTable.AsEnumerable();
            EmployeeDetailsGrid.DataContext = employeeRow;
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
    }
}