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
using HurtowniaNapojow.Windows.Employee.Warehouse.Taste;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Taste
{
    /// <summary>
    /// Interaction logic for TasteDetailsWindow.xaml
    /// </summary>
    public partial class TasteDetailsWindow
    {
        private bool _tastesFilled = false;

        public TasteDetailsWindow()
        {
            InitializeComponent();
            if (!_tastesFilled)
            {
                _tastesFilled = true;
                SetTastesBinding();
            }
        }

        public void SetTastesBinding()
        {
           TastesDataGrid.RebindContext(new SmakiTableAdapter().GetData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new TasteNewWindow(ref TastesDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tastes = TastesDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            tastes.ForEach(taste => {
             HurtowniaNapojowDataSet.SmakiRow editTaste = (HurtowniaNapojowDataSet.SmakiRow)taste.Row;
             this.OpenWindow(new TasteEditWindow(ref TastesDataGrid, ref editTaste), blockPrevious: true);
            });
            SetTastesBinding();
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
                var tastes = TastesDataGrid.SelectedItems.OfType<DataRowView>().ToList();
                tastes.ForEach(taste => DataBaseTasteHelper.DeleteTasteRow(taste.Row));
            }
            
        }
    }
}
