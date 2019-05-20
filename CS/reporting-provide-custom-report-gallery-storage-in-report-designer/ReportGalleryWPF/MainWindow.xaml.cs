using System;
using System.Windows;
using XpoReportGalleryStorage;
using DevExpress.Xpf.Core;
using DevExpress.XtraReports.UI;

namespace ReportGalleryWPF {

    public partial class MainWindow : Window {
        readonly XpoReportGalleryStorageBase storage;
        public MainWindow() {
            InitializeComponent();
            storage = new XpoReportGalleryStorageSimple();
            // Uncomment this line to register a gallery storage that saves and loads the changed items only.
            // storage = new XpoReportGalleryStorageAdvanced();
            ApplicationThemeHelper.ApplicationThemeName = Theme.Office2016WhiteSEName;
            reportDesigner.ReportGalleryOptions.Storage = storage;
            reportDesigner.OpenDocument(new XtraReport());
            Closed += OnClosed;
        }

        void OnClosed(object sender, EventArgs e) {
            storage.Dispose();
        }
    }
}
