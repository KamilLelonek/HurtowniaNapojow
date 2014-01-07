using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackageName
{
    /// <summary>
    /// Interaction logic for BulkPackageNameEditWindow.xaml
    /// </summary>
    public partial class BulkPackageNameEditWindow
    {
        private readonly DataGrid _bulkPackageNameDataGrid;
        private HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow _editBulkPackageName;

        public BulkPackageNameEditWindow(ref DataGrid bulkPackageNameDataGrid, ref HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow editBulkPackageName)
        {

            _bulkPackageNameDataGrid = bulkPackageNameDataGrid;
            _editBulkPackageName = editBulkPackageName;
            InitializeComponent();

            OldNameTextBox.Text = _editBulkPackageName.NazwaOpakowania;
         }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var newBulkPackageName = NewNameTextBox.Text;

            var result = DataBaseBulkPackageNameHelper.EditBulkPackageName(_editBulkPackageName, newBulkPackageName);
            if (!result) return;

            _bulkPackageNameDataGrid.RebindContext(DataBaseBulkPackageNameHelper.GetBulkPackageNameData());
            CloseButton.PerformClick();
        }
    }
}