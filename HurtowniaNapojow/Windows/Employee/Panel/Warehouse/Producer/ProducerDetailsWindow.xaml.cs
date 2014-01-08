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
using HurtowniaNapojow.Windows.Employee.Warehouse.Producer;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Producer
{
    /// <summary>
    /// Interaction logic for ProducerDetailsWindow.xaml
    /// </summary>
    public partial class ProducerDetailsWindow
    {
        private bool _producersFilled = false;

        public ProducerDetailsWindow()
        {
            InitializeComponent();
            if (!_producersFilled)
            {
                _producersFilled = true;
                SetProducerBinding();
            }
        }

        public void SetProducerBinding()
        {
           ProducerDataGrid.RebindContext(new ProducenciTableAdapter().GetData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerNewWindow(ref ProducerDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var producers = ProducerDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (producers.Count > 0)
            {
                producers.ForEach(producer =>
                {
                    HurtowniaNapojowDataSet.ProducenciRow editProducer = (HurtowniaNapojowDataSet.ProducenciRow)producer.Row;
                    this.OpenWindow(new ProducerEditWindow(ref ProducerDataGrid, ref editProducer), blockPrevious: true);
                });
                SetProducerBinding();
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
            var producers = ProducerDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (producers.Count > 0)
            {
                if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    //do yes stuff
                    producers.ForEach(producer => DataBaseProducerHelper.DeleteProducerRow(producer.Row));
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do usunięcia, zaznacz rekord(y) przeznaczone do usunięcia.", "Uwaga");
            }
            
        }
    }
}
