using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.Customer
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
            if (_validator.AreControlsEmpty(NameTextBox, NrTelefonuTextBox, UlicaNrTextBox, MiastoKodTextBox) || !_validator.IsCityCodeValid(MiastoKodTextBox)) return;
            if (!String.IsNullOrEmpty(EmailTextBox.Text) && !_validator.IsEmailValid(EmailTextBox)) return;
            if (!String.IsNullOrEmpty(NIPTextBox.Text) && !_validator.IsNIPValid(NIPTextBox)) return;

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

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddButton.PerformClick();
            }
        }
    }
}