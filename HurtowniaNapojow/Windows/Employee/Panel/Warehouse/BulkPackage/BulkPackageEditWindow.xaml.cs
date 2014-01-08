using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackage
{
    /// <summary>
    /// Interaction logic for BulkPackageEditWindow.xaml
    /// </summary>
    public partial class BulkPackageEditWindow
    {
        private readonly DataGrid _BulkPackageDataGrid;
        private HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow _editBulkPackage;

        public BulkPackageEditWindow(ref DataGrid BulkPackageDataGrid, ref HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow editBulkPackage)
        {
            _BulkPackageDataGrid = BulkPackageDataGrid;
            _editBulkPackage = editBulkPackage;
            InitializeComponent();
            SetDataBinding();
        }

        private void SetDataBinding() {
            var bulkPackageName = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID(_editBulkPackage.id_rodzaju_opakowania_zbiorczego);
            OldNameTextBox.Text = bulkPackageName.NazwaOpakowania.ToString();
            OldCapacityTextBox.Text = _editBulkPackage.Pojemność.ToString();

            NewBulkPackageNameComboBox.SelectedValue = _editBulkPackage.id_rodzaju_opakowania_zbiorczego;
            var bulkPackageNames = DataBaseBulkPackageNameHelper.GetBulkPackageNameData();
            NewBulkPackageNameComboBox.ItemsSource = bulkPackageNames;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var newBulkPackageNameID = (int)NewBulkPackageNameComboBox.SelectedValue;
            var newBulkPackageCapacity = int.Parse(NewCapacityBulkPackageTextBox.Text);
            var bulkPackageName = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID(newBulkPackageNameID);
            
            var result = DataBaseBulkPackageHelper.EditBulkPackage(_editBulkPackage, bulkPackageName, newBulkPackageCapacity);
            if (!result) return;

            _BulkPackageDataGrid.RebindContext(DataBaseBulkPackageHelper.GetBulkPackageData());
            CloseButton.PerformClick();
        }
    }
}