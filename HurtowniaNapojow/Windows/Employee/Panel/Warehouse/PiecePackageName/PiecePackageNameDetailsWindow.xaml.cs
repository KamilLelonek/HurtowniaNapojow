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
            PiecePackageNameDataGrid.RebindContext(new NazwyOpakowaniaSztukiTableAdapter().GetData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageNameNewWindow(ref PiecePackageNameDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var PiecePackageNames = PiecePackageNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (PiecePackageNames.Count > 0)
            {
                PiecePackageNames.ForEach(PiecePackageName =>
                {
                    HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow editPiecePackageName = (HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow)PiecePackageName.Row;
                    this.OpenWindow(new PiecePackageNameEditWindow(ref PiecePackageNameDataGrid, ref editPiecePackageName), blockPrevious: true);
                });
                SetPiecePackageNameBinding();
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do edycji, zaznacz rekord(y) przeznaczone do edycji.", "Uwaga");
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var PiecePackageNames = PiecePackageNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (PiecePackageNames.Count > 0)
            {
                if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    //do yes stuff
                    PiecePackageNames.ForEach(PiecePackageName => DataBasePiecePackageNameHelper.DeletePiecePackageNameRow(PiecePackageName.Row));
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do usunięcia, zaznacz rekord(y) przeznaczone do usunięcia.", "Uwaga");
            }
        }
    }
}
