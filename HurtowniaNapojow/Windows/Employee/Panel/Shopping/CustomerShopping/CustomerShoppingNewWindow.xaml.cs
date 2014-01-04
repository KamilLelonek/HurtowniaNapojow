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
            (new CustomerShoppingDetailsWindow(_shoppingWindow, new EmployeeShopping(shopingRow))).ShowDialog();
            _shoppingWindow.SetShoppingBinding();
            Close();
        }
    }
}