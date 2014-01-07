using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackageName
{
    /// <summary>
    /// Interaction logic for PiecePackageNameNewWindow.xaml
    /// </summary>
    public partial class PiecePackageNameNewWindow
    {
        private readonly DataGrid _PiecePackageNameDataGrid;

        public PiecePackageNameNewWindow()
        {
            _PiecePackageNameDataGrid = new DataGrid();
            InitializeComponent();
        }
        public PiecePackageNameNewWindow(ref DataGrid PiecePackageNameDataGrid)
        {
            _PiecePackageNameDataGrid = PiecePackageNameDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            var newPiecePackageName = NameTextBox.Text;

            var result = DataBasePiecePackageNameHelper.AddNewPiecePackageName(newPiecePackageName);
                if (!result)
                {
                    //MessageBox.Show("Podane dane są nieprawidłowe", Globals.TITLE_ERROR);
                    return;
                }

                _PiecePackageNameDataGrid.RebindContext(DataBasePiecePackageNameHelper.GetPiecePackageNameData());
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