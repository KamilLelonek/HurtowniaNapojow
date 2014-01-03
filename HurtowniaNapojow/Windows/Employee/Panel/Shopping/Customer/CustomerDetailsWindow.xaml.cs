using System;
using System.Linq;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class CustomerDetailsWindow
    {
        private readonly HurtowniaNapojowDataSet.KlienciRow _customer;
        private readonly Validator _validator = Validator.Instance;
        private readonly ShoppingWindow _parentWindow;

        public CustomerDetailsWindow(ShoppingWindow parentWindow, ref HurtowniaNapojowDataSet.KlienciRow customerRow)
        {
            InitializeComponent();
            _customer = customerRow;
            _parentWindow = parentWindow;
            SetShoppingBinding(_customer);
        }

        private void SetShoppingBinding(HurtowniaNapojowDataSet.KlienciRow customer)
        {
            var customerShoppingTable = DataBaseShoppingHelper.GetShoppingForCustomer(customer);

            CustomerShoppingDataGrid.DataContext = customerShoppingTable;
            CustomerDetailsGrid.DataContext = customer;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(NazwaKlientaTextBox, NrTelefonuTextBox, UlicaNrTextBox)) return;
            if (!String.IsNullOrEmpty(EmailTextBox.Text) && !_validator.IsEmailValid(EmailTextBox)) return;
            DataBaseCustomerHelper.UpdateDB(_customer, "Dane klienta zaktualizowane pomyślnie");
            _parentWindow.SetCustomersBinding();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}