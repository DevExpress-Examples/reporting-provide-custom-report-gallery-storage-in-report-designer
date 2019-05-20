Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.XtraReports.ReportGallery

Namespace XpoReportGalleryStorage
    Public Class XpoReportGalleryStorageSimple
        Inherits XpoReportGalleryStorageBase

        ' Enable synchronization with a remote storage.
        Public Overrides ReadOnly Property EnableSynchronization() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides Sub SaveGallery(ByVal gallery As Gallery)
            Using unitOfWork As New UnitOfWork(DataLayer)
                unitOfWork.BeginTransaction()
                Dim deletedItems = unitOfWork.Query(Of ReportGalleryTable)().Select(Function(a) a)
                For Each deletes In deletedItems
                    deletes.Delete()
                Next deletes
                unitOfWork.CommitTransaction()
                unitOfWork.BeginTransaction()
                For Each newItem In gallery.Items
                    MapFromGalleryItem(newItem, unitOfWork)
                Next newItem
                unitOfWork.CommitTransaction()
            End Using
        End Sub

        Public Overrides Function TryLoadGallery(<System.Runtime.InteropServices.Out()> ByRef gallery As Gallery) As Boolean
            Using unitOfWork As New UnitOfWork(DataLayer)
                gallery = New Gallery()
                Dim remoteItems = unitOfWork.Query(Of ReportGalleryTable)().Select(Function(a) MapFromTableRow(a)).ToList()
                If Not remoteItems.Any() Then
                    Return False
                End If
                gallery.Items.AddRange(remoteItems)
                Return True
            End Using
        End Function
    End Class
End Namespace
