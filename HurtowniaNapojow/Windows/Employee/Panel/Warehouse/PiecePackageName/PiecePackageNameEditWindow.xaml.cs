using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackageName
{
    /// <summary>
    /// Interaction logic for PiecePackageNameEditWindow.xaml
    /// </summary>
    public partial class PiecePackageNameEditWindow
    {
        private readonly DataGrid _PiecePackageNameDataGrid;
        private HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow _editPiecePackageName;

        public PiecePackageNameEditWindow(ref DataGrid PiecePackageNameDataGrid, ref HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow editPiecePackageName)
        {

            _PiecePackageNameDataGrid = PiecePackageNameDataGrid;
            _editPiecePackageName = editPiecePackageName;
            InitializeComponent();

            OldNameTextBox.Text = _editPiecePackageName.NazwaOpakowania;
         }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var newPiecePackageName = NewNameTextBox.Text;

            var result = DataBasePiecePackageNameHelper.EditPiecePackageName(_editPiecePackageName, newPiecePackageName);
            if (!result) return;

            _PiecePackageNameDataGrid.RebindContext(DataBasePiecePackageNameHelper.GetPiecePackageNameData());
            CloseButton.PerformClick();
        }
    }
}