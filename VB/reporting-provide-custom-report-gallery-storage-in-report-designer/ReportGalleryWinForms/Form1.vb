Imports XpoReportGalleryStorage
Imports DevExpress.XtraReports.Extensions
Imports System.Windows.Forms

Namespace ReportGalleryWinForms

    Public Partial Class Form1
        Inherits Form

        Private xpoGalleryStorage As XpoReportGalleryStorageBase

        Public Sub New()
            InitializeComponent()
            xpoGalleryStorage = New XpoReportGalleryStorageSimple()
            'Uncomment thisline to register a gallery storage that saves and loads the changeditems only.
            'xpoGalleryStorage = new XpoReportGalleryStorageAdvanced();
            ReportGalleryExtension.RegisterExtensionGlobal(xpoGalleryStorage)
            AddHandler FormClosed, AddressOf OnFormClosed
        End Sub

        Private Overloads Sub OnFormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
            xpoGalleryStorage.Dispose()
        End Sub
    End Class
End Namespace
