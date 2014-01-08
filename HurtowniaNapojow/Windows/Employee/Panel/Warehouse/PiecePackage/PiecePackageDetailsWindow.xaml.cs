using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse;
using HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackage;


namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.PiecePackage
{
    /// <summary>
    /// Interaction logic for PiecePackageDetailsWindow.xaml
    /// </summary>
    public partial class PiecePackageDetailsWindow
    {
        private bool _PiecePackagesFilled = false;

        public PiecePackageDetailsWindow()
        {
            InitializeComponent();
            if (!_PiecePackagesFilled)
            {
                _PiecePackagesFilled = true;
                SetPiecePackageBinding();
            }
        }

        public void SetPiecePackageBinding()
        {
            var piecePackageTable = Helpers.PiecePackageHelper.GetPiecePackage();
            PiecePackageDataGrid.DataContext = piecePackageTable;
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageNewWindow(ref PiecePackageDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var piecePackages = PiecePackageDataGrid.SelectedItems.Cast<PiecePackageHelper>().ToList();
            piecePackages.ForEach(piecePackage =>
            {
                var piecePackageToEdit = DataBasePiecePackageHelper.GetPiecePackageByID(piecePackage.Id);
                HurtowniaNapojowDataSet.OpakowaniaSztukiRow editPiecePackage = (HurtowniaNapojowDataSet.OpakowaniaSztukiRow)piecePackageToEdit;
                this.OpenWindow(new PiecePackageEditWindow(ref PiecePackageDataGrid, ref editPiecePackage), blockPrevious: true);
            });
            SetPiecePackageBinding();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                //do yes stuff
                var PiecePackages = PiecePackageDataGrid.SelectedItems.Cast<PiecePackageHelper>().ToList();
                PiecePackages.ForEach(piecePackage =>
                                    {
                                        var piecePackageToDelete = DataBasePiecePackageHelper.GetPiecePackageByID(piecePackage.Id);
                                        DataBasePiecePackageHelper.DeletePiecePackageRow(piecePackageToDelete);
                                    } );
                SetPiecePackageBinding();
            }
            
        }
    }
}
