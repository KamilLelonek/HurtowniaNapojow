using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackage
{
    /// <summary>
    /// Interaction logic for BulkPackageNewWindow.xaml
    /// </summary>
    public partial class BulkPackageNewWindow
    {
        private readonly DataGrid _BulkPackageDataGrid;
        private readonly Validator _validator = Validator.Instance;

        public BulkPackageNewWindow()
        {
            _BulkPackageDataGrid = new DataGrid();
            InitializeComponent();
            SetDataBinding();
        }
        public BulkPackageNewWindow(ref DataGrid BulkPackageDataGrid)
        {
            _BulkPackageDataGrid = BulkPackageDataGrid;
            InitializeComponent();
            SetDataBinding();
        }

        private void SetDataBinding()
        {
            var bulkPackageNames = DataBaseBulkPackageNameHelper.GetBulkPackageNameData();
            NameBulkPackageComboBox.ItemsSource = bulkPackageNames;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(CapacityBulkPackageTextBox)) return;
            if (_validator.IsFloatValid(CapacityBulkPackageTextBox)) return;
            if (_validator.AreComboBoxEmpty(NameBulkPackageComboBox)) return;

            var newBulkPackageID = (int)NameBulkPackageComboBox.SelectedValue;
            var newBulkPackageCapacity = (int)float.Parse(CapacityBulkPackageTextBox.Text);
            try
            {
                var bulkPackageName = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID(newBulkPackageID);
                var result = DataBaseBulkPackageHelper.AddNewBulkPackage(bulkPackageName, newBulkPackageCapacity);
                if (!result)
                {
                    MessageBox.Show("Podane dane są nieprawidłowe", Globals.TITLE_ERROR);
                    return;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Błąd", Globals.TITLE_ERROR);
            }

            _BulkPackageDataGrid.RebindContext(BulkPackageHelper.GetBulkPackage());
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