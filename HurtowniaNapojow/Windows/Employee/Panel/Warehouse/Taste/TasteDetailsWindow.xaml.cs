using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
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
           TastesDataGrid.RebindContext(DataBaseTasteHelper.GetTastesData());
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
