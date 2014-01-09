using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackage
{
    /// <summary>
    /// Interaction logic for PiecePackageEditWindow.xaml
    /// </summary>
    public partial class PiecePackageEditWindow
    {
        private readonly DataGrid _PiecePackageDataGrid;
        private HurtowniaNapojowDataSet.OpakowaniaSztukiRow _editPiecePackage;
        private readonly Validator _validator = Validator.Instance;

        public PiecePackageEditWindow(ref DataGrid PiecePackageDataGrid, ref HurtowniaNapojowDataSet.OpakowaniaSztukiRow editPiecePackage)
        {
            _PiecePackageDataGrid = PiecePackageDataGrid;
            _editPiecePackage = editPiecePackage;
            InitializeComponent();
            SetDataBinding();
        }

        private void SetDataBinding()
        {
            var piecePackageName = DataBasePiecePackageNameHelper.GetPiecePackageNameByID(_editPiecePackage.id_rodzaju_opakowania_sztuki);
            OldNameTextBox.Text = piecePackageName.NazwaOpakowania.ToString();
            OldCapacityTextBox.Text = _editPiecePackage.Pojemność.ToString();

            NewPiecePackageNameComboBox.SelectedValue = _editPiecePackage.id_rodzaju_opakowania_sztuki;
            var piecePackageNames = DataBasePiecePackageNameHelper.GetPiecePackageNameData();
            NewPiecePackageNameComboBox.ItemsSource = piecePackageNames;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(NewCapacityPiecePackageTextBox)) return;
            if (_validator.IsFloatValid(NewCapacityPiecePackageTextBox)) return;
            if (_validator.AreComboBoxEmpty(NewPiecePackageNameComboBox)) return;

            var newPiecePackageID = (int)NewPiecePackageNameComboBox.SelectedValue;
            var newPiecePackageCapacity = float.Parse(NewCapacityPiecePackageTextBox.Text);
            
            var piecePackageName = DataBasePiecePackageNameHelper.GetPiecePackageNameByID(newPiecePackageID);
            var result = DataBasePiecePackageHelper.EditPiecePackage(_editPiecePackage, piecePackageName, newPiecePackageCapacity);
            if (!result) return;

            _PiecePackageDataGrid.RebindContext(DataBasePiecePackageHelper.GetPiecePackageData());
            CloseButton.PerformClick();
        }
    }
}