using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackage
{
    /// <summary>
    /// Interaction logic for PiecePackageNewWindow.xaml
    /// </summary>
    public partial class PiecePackageNewWindow
    {
        private readonly DataGrid _PiecePackageDataGrid;
        private readonly Validator _validator = Validator.Instance;

        public PiecePackageNewWindow()
        {
            _PiecePackageDataGrid = new DataGrid();
            InitializeComponent();
            SetDataBinding();
        }
        public PiecePackageNewWindow(ref DataGrid PiecePackageDataGrid)
        {
            _PiecePackageDataGrid = PiecePackageDataGrid;
            InitializeComponent();
            SetDataBinding();
        }

        private void SetDataBinding() 
        { 
            var piecePackageNames = DataBasePiecePackageNameHelper.GetPiecePackageNameData();
            NamePiecePackageComboBox.ItemsSource= piecePackageNames;
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(CapacityPiecePackageTextBox)) return;
            if (_validator.IsFloatValid(CapacityPiecePackageTextBox)) return;
            //if (NamePiecePackageComboBox.SelectedValue == null)
            //{
            //    MessageBox.Show("Null");
            //}
            //else {
            //    MessageBox.Show(NamePiecePackageComboBox.SelectedValue.ToString());
            //}

            var newPiecePackageID = (int)NamePiecePackageComboBox.SelectedValue;
            var newPiecePackageCapacity = float.Parse(CapacityPiecePackageTextBox.Text);
            try
            {
                var piecePackageName = DataBasePiecePackageNameHelper.GetPiecePackageNameByID(newPiecePackageID);
                var result = DataBasePiecePackageHelper.AddNewPiecePackage(piecePackageName, newPiecePackageCapacity);
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

            _PiecePackageDataGrid.RebindContext(PiecePackageHelper.GetPiecePackage());
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