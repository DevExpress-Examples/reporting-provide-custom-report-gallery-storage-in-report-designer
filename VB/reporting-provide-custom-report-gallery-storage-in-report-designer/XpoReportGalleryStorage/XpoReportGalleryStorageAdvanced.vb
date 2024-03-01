Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.XtraReports.ReportGallery
Imports System.Runtime.InteropServices

Namespace XpoReportGalleryStorage

    Public Class XpoReportGalleryStorageAdvanced
        Inherits XpoReportGalleryStorage.XpoReportGalleryStorageBase

        ' Enable synchronization with a remote storage.
        Public Overrides ReadOnly Property EnableSynchronization As Boolean
            Get
                Return True
            End Get
        End Property

        ' This method's default implementation loads all gallery items from the storage and compares them with all local items.
        ' In this example, this method is overridden to get gallery changes only.  
        Protected Overrides Function GetChanges(ByVal galleryItems As System.Collections.Generic.IEnumerable(Of DevExpress.XtraReports.ReportGallery.GalleryItem)) As GalleryChanges
            Dim remoteHashList = Me.LoadHashList()
            Dim localHashList = galleryItems.[Select](Function(a) a.GetHashCode())
            Dim hashListToDelete = localHashList.Except(remoteHashList).ToList()
            Dim hashListToAdd = remoteHashList.Except(localHashList).ToList()
            Dim newItems = Me.LoadNewItems(hashListToAdd).ToList()
            Dim deletedItems = hashListToDelete.Join(galleryItems, Function(hash) hash, Function(localGallery) localGallery.GetHashCode(), Function(innerResult, outerResult) outerResult).ToList()
            Return New DevExpress.XtraReports.Extensions.ReportGalleryExtension.GalleryChanges() With {.NewItems = newItems, .OldItems = deletedItems}
        End Function

        ' The SaveGallery method overwrites the entire gallery.
        ' This method is internally called in the SaveGalleryChanges method's default implementation.
        ' If you do not override the SaveGalleryChanges method, uncomment the lines below.
        Public Overrides Sub SaveGallery(ByVal gallery As DevExpress.XtraReports.ReportGallery.Gallery)
        'using(UnitOfWork unitOfWork = new UnitOfWork(DataLayer)) {
        '    unitOfWork.BeginTransaction();
        '    var deletedItems = unitOfWork.Query<ReportGalleryTable>().Select(a => a);
        '    foreach(var deletes in deletedItems) {
        '        deletes.Delete();
        '    }
        '    unitOfWork.CommitTransaction();
        '    unitOfWork.BeginTransaction();
        '    foreach(var newItem in gallery.Items)
        '        MapFromGalleryItem(newItem, unitOfWork);
        '    unitOfWork.CommitTransaction();
        '}
        End Sub

        ' Override the SaveGalleryChanges method to save gallery changes only (not the entire gallery).
        ' If you do not override this method, it internally calls the SaveGallery method.
        Public Overrides Sub SaveGalleryChanges(ByVal gallery As DevExpress.XtraReports.ReportGallery.Gallery, ByVal changedItems As System.Collections.Generic.IEnumerable(Of DevExpress.XtraReports.ReportGallery.GalleryItem), ByVal changeType As DevExpress.XtraReports.ReportGallery.GalleryChangeType)
            Using unitOfWork = New DevExpress.Xpo.UnitOfWork(Me.DataLayer)
                unitOfWork.BeginTransaction()
                Dim ids = changedItems.[Select](Function(a) Integer.Parse(a.ID)).ToList()
                Dim queryResult = unitOfWork.QueryInTransaction(Of XpoReportGalleryStorage.ReportGalleryTable)().Where(Function(a) ids.Contains(a.ID))
                For Each item In changedItems
                    Dim changedItem = queryResult.FirstOrDefault(Function(a) a.ID = Integer.Parse(item.ID))
                    If changeType = DevExpress.XtraReports.ReportGallery.GalleryChangeType.Delete Then changedItem.Delete()
                    If changeType = DevExpress.XtraReports.ReportGallery.GalleryChangeType.Update Then
                        changedItem.SID = item.SID
                        changedItem.DisplayName = item.DisplayName
                        changedItem.Hash = item.GetHashCode()
                        changedItem.ContentHash = item.ContentHash
                        changedItem.ItemType = item.ItemType
                    End If

                    If changeType = DevExpress.XtraReports.ReportGallery.GalleryChangeType.Add Then Me.MapFromGalleryItem(item, unitOfWork)
                Next

                Try
                    unitOfWork.CommitTransaction()
                Catch
                    unitOfWork.RollbackTransaction()
                End Try
            End Using
        End Sub

        Public Overrides Function TryLoadGallery(<Out> ByRef gallery As DevExpress.XtraReports.ReportGallery.Gallery) As Boolean
            Using unitOfWork As DevExpress.Xpo.UnitOfWork = New DevExpress.Xpo.UnitOfWork(Me.DataLayer)
                gallery = New DevExpress.XtraReports.ReportGallery.Gallery()
                Dim remoteItems = unitOfWork.Query(Of XpoReportGalleryStorage.ReportGalleryTable)().[Select](Function(a) Me.MapFromTableRow(a)).ToList()
                If Not remoteItems.Any() Then Return False
                gallery.Items.AddRange(remoteItems)
                Return True
            End Using
        End Function

        Private Function LoadHashList() As IEnumerable(Of Integer)
            Using unitOfWork As DevExpress.Xpo.UnitOfWork = New DevExpress.Xpo.UnitOfWork(Me.DataLayer)
                Dim remoteHashList = unitOfWork.Query(Of XpoReportGalleryStorage.ReportGalleryTable)().[Select](Function(a) a.Hash).ToList()
                Return remoteHashList
            End Using
        End Function

        Private Function LoadNewItems(ByVal newItemsHashKeys As System.Collections.Generic.IEnumerable(Of Integer)) As IEnumerable(Of DevExpress.XtraReports.ReportGallery.GalleryItem)
            Using unitOfWork As DevExpress.Xpo.UnitOfWork = New DevExpress.Xpo.UnitOfWork(Me.DataLayer)
                Dim remoteItems = unitOfWork.Query(Of XpoReportGalleryStorage.ReportGalleryTable)().Where(Function(a) newItemsHashKeys.Contains(a.Hash)).[Select](Function(a) Me.MapFromTableRow(a)).ToList()
                Return remoteItems
            End Using
        End Function
    End Class
End Namespace
