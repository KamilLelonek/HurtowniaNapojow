using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class CustomerShoppingNewWindow
    {
        private readonly ShoppingWindow _shoppingWindow;

        public CustomerShoppingNewWindow(ShoppingWindow shoppingWindow)
        {
            InitializeComponent();
            _shoppingWindow = shoppingWindow;
            CustomersDataGrid.RebindContext(new KlienciTableAdapter().GetData());
            SetCustomersComponentsEvents();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CustomersChoose_Clicked(object sender, RoutedEventArgs e)
        {
            var customerRow = (((DataRowView)((Button)sender).DataContext)).Row as HurtowniaNapojowDataSet.KlienciRow;
            if (customerRow == null) return;
            
            HurtowniaNapojowDataSet.ZakupyKlientaRow shopingRow = DataBaseShoppingHelper.AddNewShopping(SessionHelper.Instance.CurrentEmployee, customerRow);
            (new CustomerShoppingDetailsWindow(_shoppingWindow, new EmployeeShopping(ref shopingRow))).ShowDialog();
            _shoppingWindow.SetShoppingBinding();
            Close();
        }

        private void SetCustomersComponentsEvents()
        {
            CustomersFilterTextBox.TextChanged += (sender, args) => CustomersFilterChanged();
            CustomersFilterComboBox.SelectionChanged += (sender, args) => CustomersFilterChanged();
        }
        private void CustomersResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            CustomersFilterComboBox.Text = Globals.FILTER_SELECT;
            CustomersFilterTextBox.Text = "";
            ((DataTable)CustomersDataGrid.DataContext).DefaultView.RowFilter = "";
        }
        private void CustomersFilterChanged()
        {
            var comboBoxItem = CustomersFilterComboBox.SelectedItem as ComboBoxItem;
            if (comboBoxItem == null) CustomersFilterComboBox.SelectedIndex = 0;
            comboBoxItem = CustomersFilterComboBox.SelectedItem as ComboBoxItem;

            var filterType = comboBoxItem.Content.ToString();
            var filterValue = CustomersFilterTextBox.Text;
            var filter = String.Format("{0} LIKE '%{1}%'", filterType, filterValue);
            ((DataTable)CustomersDataGrid.DataContext).DefaultView.RowFilter = filter;
        }
    }
}