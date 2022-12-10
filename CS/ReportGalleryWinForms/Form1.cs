using XpoReportGalleryStorage;
using DevExpress.XtraReports.Extensions;
using System.Windows.Forms;

namespace ReportGalleryWinForms {
    public partial class Form1 : Form {
        XpoReportGalleryStorageBase xpoGalleryStorage;
        public Form1() {
            InitializeComponent();
            xpoGalleryStorage = new XpoReportGalleryStorageSimple();
            //Uncomment thisline to register a gallery storage that saves and loads the changeditems only.
            //xpoGalleryStorage = new XpoReportGalleryStorageAdvanced();
            ReportGalleryExtension.RegisterExtensionGlobal(xpoGalleryStorage);
            FormClosed += OnFormClosed;
        }

        void OnFormClosed(object sender, FormClosedEventArgs e) {
            xpoGalleryStorage.Dispose();
        }
    }
}
