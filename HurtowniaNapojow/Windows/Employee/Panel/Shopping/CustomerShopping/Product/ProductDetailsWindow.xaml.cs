using System;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping.Product
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class ProductDetailsWindow
    {
        private readonly HurtowniaNapojowDataSet.ProduktyKlientaRow _product;
        private readonly Validator _validator = Validator.Instance;
        private readonly CustomerShoppingDetailsWindow _parentWindow;

        public ProductDetailsWindow(CustomerShoppingDetailsWindow parentWindow, ref HurtowniaNapojowDataSet.ProduktyKlientaRow productRow)
        {
            InitializeComponent();
            _product = productRow;
            _parentWindow = parentWindow;
        }

      

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.SetShoppingBinding();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}