using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping.Product
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class ProductNewWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private readonly DataGrid _customerDataGrid;

        public ProductNewWindow(ref DataGrid customerDataGrid)
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
            CloseButton.PerformClick();
        }
    }
}