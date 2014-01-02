using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class CustomerNewWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private readonly DataGrid _customerDataGrid;

        public CustomerNewWindow(ref DataGrid customerDataGrid)
        {
            _customerDataGrid = customerDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(NameTextBox, NrTelefonuTextBox, UlicaNrTextBox)) return;
            if (!String.IsNullOrEmpty(EmailTextBox.Text) && !_validator.IsEmailValid(EmailTextBox)) return;

            var newNazwaKlienta = NameTextBox.Text;
            var newNIP = NIPTextBox.Text;
            var newNrTelefonu = NrTelefonuTextBox.Text;
            var newEmail = EmailTextBox.Text;
            var newUlicaNr = UlicaNrTextBox.Text;
            var newMiastoKod = MiastoKodTextBox.Text;

            var result = DataBaseCustomerHelper.AddNewCustomer(newNazwaKlienta, newNIP, newNrTelefonu, newEmail, newUlicaNr, newMiastoKod);
            if (!result) return;

            _customerDataGrid.RebindContext(DataBaseCustomerHelper.GetCustomersData());
            CloseButton.PerformClick();
        }
    }
}