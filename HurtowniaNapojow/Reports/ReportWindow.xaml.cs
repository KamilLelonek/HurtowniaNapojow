using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using HurtowniaNapojow.Helpers;
using Microsoft.Reporting.WinForms;

namespace HurtowniaNapojow.Reports
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private Boolean _isReportViewerLoaded;
        public ReportWindow(IEnumerable dataTable, String reportRelativePath)
        {
            InitializeComponent();
            ReportViewer.Load +=
                (sender, args) => ReportViewerOnLoad(dataTable, reportRelativePath);
        }


        private void ReportViewerOnLoad(IEnumerable dataTable, String reportPath)
        {
            if (_isReportViewerLoaded) return;

            var reportDataSource = new ReportDataSource("HurtowniaNapojowDataSet", dataTable);
            ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            ReportViewer.LocalReport.ReportPath = String.Format(@"../../Reports/{0}", reportPath);
            ReportViewer.RefreshReport();
            _isReportViewerLoaded = true;
        }
    }
}