using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.Customer
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class CustomerNewWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private readonly ShoppingWindow _shoppingWindow;

        public CustomerNewWindow(ShoppingWindow shoppingWindow)
        {
            _shoppingWindow = shoppingWindow;
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
            var newNIP = String.IsNullOrEmpty(NIPTextBox.Text) ? null : NIPTextBox.Text;
            var newNrTelefonu = NrTelefonuTextBox.Text;
            var newEmail = String.IsNullOrEmpty(EmailTextBox.Text) ? null : EmailTextBox.Text;
            var newUlicaNr = UlicaNrTextBox.Text;
            var newMiastoKod = MiastoKodTextBox.Text;

            var result = DataBaseCustomerHelper.AddNewCustomer(newNazwaKlienta, newNIP, newNrTelefonu, newEmail, newUlicaNr, newMiastoKod);
            if (!result) return;

            _shoppingWindow.SetCustomersBinding();
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