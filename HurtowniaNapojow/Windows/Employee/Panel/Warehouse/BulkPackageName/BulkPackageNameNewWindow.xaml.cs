using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackageName
{
    /// <summary>
    /// Interaction logic for BulkPackageNameNewWindow.xaml
    /// </summary>
    public partial class BulkPackageNameNewWindow
    {
        private readonly DataGrid _bulkPackageNameDataGrid;

        public BulkPackageNameNewWindow()
        {
            _bulkPackageNameDataGrid = new DataGrid();
            InitializeComponent();
        }
        public BulkPackageNameNewWindow(ref DataGrid bulkPackageNameDataGrid)
        {
            _bulkPackageNameDataGrid = bulkPackageNameDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            var newBulkPackageName = NameTextBox.Text;

            var result = DataBaseBulkPackageNameHelper.AddNewBulkPackageName(newBulkPackageName);
                if (!result)
                {
                    //MessageBox.Show("Podane dane są nieprawidłowe", Globals.TITLE_ERROR);
                    return;
                }

                _bulkPackageNameDataGrid.RebindContext(DataBaseBulkPackageNameHelper.GetBulkPackageNameData());
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