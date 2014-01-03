using System;
using System.Linq;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class CustomerShoppingDetailsWindow
    {
        private readonly EmployeeShopping _shopping;
        private readonly ShoppingWindow _shoppingWindow;

        public CustomerShoppingDetailsWindow(ShoppingWindow shoppingWindow, EmployeeShopping shopping)
        {
            InitializeComponent();
            _shopping = shopping;
            _shoppingWindow = shoppingWindow;
            SetShoppingBinding();
        }

        private void SetShoppingBinding()
        {
            CustomerDetailsGrid.DataContext = _shopping;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //if (_validator.AreControlsEmpty(NazwaKlientaTextBox, NrTelefonuTextBox, UlicaNrTextBox)) return;
            //if (!String.IsNullOrEmpty(EmailTextBox.Text) && !_validator.IsEmailValid(EmailTextBox)) return;
            //DataBaseCustomerHelper.UpdateDB(_customer, "Dane klienta zaktualizowane pomyślnie");
            _shoppingWindow.SetShoppingBinding();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}