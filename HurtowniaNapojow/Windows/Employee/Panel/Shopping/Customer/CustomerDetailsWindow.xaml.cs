using System;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.Customer
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class CustomerDetailsWindow
    {
        private readonly HurtowniaNapojowDataSet.KlienciRow _customer;
        private readonly Validator _validator = Validator.Instance;
        private readonly ShoppingWindow _parentWindow;

        public CustomerDetailsWindow(ShoppingWindow parentWindow, HurtowniaNapojowDataSet.KlienciRow customerRow)
        {
            InitializeComponent();
            _customer = customerRow;
            _parentWindow = parentWindow;
            SetShoppingBinding(_customer);
        }

        private void SetShoppingBinding(HurtowniaNapojowDataSet.KlienciRow customer)
        {
            var customerShoppingTable = Helpers.CustomerShopping.GetCustomerShoppings(customer);
            CustomerShoppingDataGrid.DataContext = customerShoppingTable;
            CustomerDetailsGrid.DataContext = customer;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(NazwaKlientaTextBox, NrTelefonuTextBox, UlicaNrTextBox, MiastoKodTextBox) || !_validator.IsCityCodeValid(MiastoKodTextBox)) return;
            if (!String.IsNullOrEmpty(EmailTextBox.Text) && !_validator.IsEmailValid(EmailTextBox)) return;
            if (!String.IsNullOrEmpty(NIPTextBox.Text) && !_validator.IsNIPValid(NIPTextBox)) return;
            _customer.NazwaKlienta = NazwaKlientaTextBox.Text;
            _customer.NrTelefonu = NrTelefonuTextBox.Text;
            _customer.UlicaNumer = UlicaNrTextBox.Text;
            _customer.MiastoKod = MiastoKodTextBox.Text;
            _customer.Email = String.IsNullOrEmpty(EmailTextBox.Text) ? null : EmailTextBox.Text;
            _customer.NIP = String.IsNullOrEmpty(NIPTextBox.Text) ? null : NIPTextBox.Text;
            DataBaseCustomerHelper.UpdateDB(_customer, "Dane klienta zaktualizowane pomyślnie");
            _parentWindow.SetCustomersBinding();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}