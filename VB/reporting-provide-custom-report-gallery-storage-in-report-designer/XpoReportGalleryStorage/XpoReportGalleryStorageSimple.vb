Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.XtraReports.ReportGallery
Imports System.Runtime.InteropServices

Namespace XpoReportGalleryStorage

    Public Class XpoReportGalleryStorageSimple
        Inherits XpoReportGalleryStorageBase

        ' Enable synchronization with a remote storage.
        Public Overrides ReadOnly Property EnableSynchronization As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides Sub SaveGallery(ByVal gallery As Gallery)
            Using unitOfWork As UnitOfWork = New UnitOfWork(DataLayer)
                unitOfWork.BeginTransaction()
                Dim deletedItems = unitOfWork.Query(Of ReportGalleryTable)().[Select](Function(a) a)
                For Each deletes In deletedItems
                    deletes.Delete()
                Next

                unitOfWork.CommitTransaction()
                unitOfWork.BeginTransaction()
                For Each newItem In gallery.Items
                    MapFromGalleryItem(newItem, unitOfWork)
                Next

                unitOfWork.CommitTransaction()
            End Using
        End Sub

        Public Overrides Function TryLoadGallery(<Out> ByRef gallery As Gallery) As Boolean
            Using unitOfWork As UnitOfWork = New UnitOfWork(DataLayer)
                gallery = New Gallery()
                Dim remoteItems = unitOfWork.Query(Of ReportGalleryTable)().[Select](Function(a) MapFromTableRow(a)).ToList()
                If Not remoteItems.Any() Then Return False
                gallery.Items.AddRange(remoteItems)
                Return True
            End Using
        End Function
    End Class
End Namespace
