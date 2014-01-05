using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class EmployeeNewWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private readonly DataGrid _employeesDataGrid;

        public EmployeeNewWindow(ref DataGrid employeesDataGrid)
        {
            _employeesDataGrid = employeesDataGrid;
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

            _employeesDataGrid.RebindContext(DataBaseEmployeeHelper.GetEmployeesData());
            CloseButton.PerformClick();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddButton.PerformClick();
            }
        }
    }
}