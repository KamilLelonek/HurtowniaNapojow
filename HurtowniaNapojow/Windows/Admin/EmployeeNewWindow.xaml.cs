using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class EmployeeNewWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private readonly DataGrid EmployeesDataGrid;

        public EmployeeNewWindow(ref DataGrid employeesDataGrid)
        {
            EmployeesDataGrid = employeesDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(FirstNameTextBox, LastNameTextBox, EmailTextBox) ||
                !_validator.IsEmailValid(EmailTextBox)) return;

            var newFirstName = FirstNameTextBox.Text;
            var newLastName = LastNameTextBox.Text;
            var newEmail = EmailTextBox.Text;
            var isAdmin = AdminCheckBox.IsChecked.GetValueOrDefault(false);

            var result = DataBaseEmployeeHelper.AddNewEmployee(newFirstName, newLastName, newEmail, isAdmin);
            if (!result) return;

            EmployeesDataGrid.RebindContext(DataBaseEmployeeHelper.GetEmployeesData());
            CloseButton.PerformClick();
        }
    }
}