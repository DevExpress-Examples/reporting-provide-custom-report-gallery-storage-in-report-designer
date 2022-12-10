Imports System
Imports System.Windows
Imports XpoReportGalleryStorage
Imports DevExpress.Xpf.Core
Imports DevExpress.XtraReports.UI

Namespace ReportGalleryWPF

	Partial Public Class MainWindow
		Inherits Window

		Private ReadOnly storage As XpoReportGalleryStorageBase
		Public Sub New()
			InitializeComponent()
			storage = New XpoReportGalleryStorageSimple()
			' Uncomment this line to register a gallery storage that saves and loads the changed items only.
			' storage = new XpoReportGalleryStorageAdvanced();
			ApplicationThemeHelper.ApplicationThemeName = Theme.Office2016WhiteSEName
			reportDesigner.ReportGalleryOptions.Storage = storage
			reportDesigner.OpenDocument(New XtraReport())
			AddHandler Closed, AddressOf OnClosed
		End Sub

		Private Overloads Sub OnClosed(ByVal sender As Object, ByVal e As EventArgs)
			storage.Dispose()
		End Sub
	End Class
End Namespace
