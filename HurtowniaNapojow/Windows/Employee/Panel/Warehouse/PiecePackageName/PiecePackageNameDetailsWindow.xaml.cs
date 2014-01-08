using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackageName;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.PiecePackageName
{
    /// <summary>
    /// Interaction logic for PiecePackageNameDetailsWindow.xaml
    /// </summary>
    public partial class PiecePackageNameDetailsWindow
    {
        private bool _PiecePackageNamesFilled = false;

        public PiecePackageNameDetailsWindow()
        {
            InitializeComponent();
            if (!_PiecePackageNamesFilled)
            {
                _PiecePackageNamesFilled = true;
                SetPiecePackageNameBinding();
            }
        }

        public void SetPiecePackageNameBinding()
        {
            PiecePackageNameDataGrid.RebindContext(DataBasePiecePackageNameHelper.GetPiecePackageNameData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageNameNewWindow(ref PiecePackageNameDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var PiecePackageNames = PiecePackageNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            PiecePackageNames.ForEach(PiecePackageName =>
            {
                HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow editPiecePackageName = (HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow)PiecePackageName.Row;
                this.OpenWindow(new PiecePackageNameEditWindow(ref PiecePackageNameDataGrid, ref editPiecePackageName), blockPrevious: true);
            });
            SetPiecePackageNameBinding();
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
                var PiecePackageNames = PiecePackageNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
                PiecePackageNames.ForEach(PiecePackageName => DataBasePiecePackageNameHelper.DeletePiecePackageNameRow(PiecePackageName.Row));
            }
            
        }
    }
}
