Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata
Imports DevExpress.XtraReports.Extensions
Imports DevExpress.XtraReports.ReportGallery
Imports System
Imports System.IO

Namespace XpoReportGalleryStorage

    Public MustInherit Class XpoReportGalleryStorageBase
        Inherits DevExpress.XtraReports.Extensions.ReportGalleryExtension
        Implements System.IDisposable

        Private dataLayerField As DevExpress.Xpo.ThreadSafeDataLayer

        Private disposableObjects As System.IDisposable()

        Protected ReadOnly Property DataLayer As ThreadSafeDataLayer
            Get
                Return If(Me.dataLayerField, Function()
                    Me.dataLayerField = Me.CreateDataLayer()
                    Return Me.dataLayerField
                End Function())
            End Get
        End Property

        Protected Overridable ReadOnly Property ConnectionString As String = DevExpress.Xpo.DB.AccessConnectionProvider.GetConnectionString(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data\ReportGallery.mdb"))

        Private Function CreateDataLayer() As ThreadSafeDataLayer
            Dim dictionary = XpoReportGalleryStorage.XpoReportGalleryStorageBase.PrepareDictionary()
            Using updateDataLayer = DevExpress.Xpo.XpoDefault.GetDataLayer(Me.ConnectionString, dictionary, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema)
                updateDataLayer.UpdateSchema(False, dictionary.CollectClassInfos(GetType(XpoReportGalleryStorage.ReportGalleryTable)))
                Using unitOfWork = New DevExpress.Xpo.UnitOfWork(updateDataLayer)
                    unitOfWork.CreateObjectTypeRecords(GetType(XpoReportGalleryStorage.ReportGalleryTable))
                End Using
            End Using

            Dim dataStore = DevExpress.Xpo.XpoDefault.GetConnectionProvider(Me.ConnectionString, DevExpress.Xpo.DB.AutoCreateOption.SchemaAlreadyExists, Me.disposableObjects)
            Return New DevExpress.Xpo.ThreadSafeDataLayer(dictionary, dataStore)
        End Function

        Public Sub Dispose() Implements Global.System.IDisposable.Dispose
            If Me.dataLayerField IsNot Nothing Then
                Me.dataLayerField.Dispose()
                Me.dataLayerField = Nothing
            End If

            If Me.disposableObjects IsNot Nothing Then
                For Each obj In Me.disposableObjects
                    obj.Dispose()
                Next

                Me.disposableObjects = Nothing
            End If
        End Sub

        Protected Function MapFromTableRow(ByVal row As XpoReportGalleryStorage.ReportGalleryTable) As GalleryItem
            Dim galleryItem = New DevExpress.XtraReports.ReportGallery.GalleryItem() With {.Content = row.Content, .ContentHash = row.ContentHash, .DisplayName = row.DisplayName, .ID = row.ID.ToString(), .ItemKind = row.ItemKind, .SID = row.SID, .ItemType = row.ItemType}
            Return galleryItem
        End Function

        Protected Function MapFromGalleryItem(ByVal galleryItem As DevExpress.XtraReports.ReportGallery.GalleryItem, ByVal session As DevExpress.Xpo.Session) As ReportGalleryTable
            Dim table = New XpoReportGalleryStorage.ReportGalleryTable(session) With {.ID = Integer.Parse(galleryItem.ID), .SID = galleryItem.SID, .ItemKind = galleryItem.ItemKind, .DisplayName = galleryItem.DisplayName, .Content = galleryItem.Content, .Hash = galleryItem.GetHashCode(), .ContentHash = galleryItem.ContentHash, .ItemType = galleryItem.ItemType}
            Return table
        End Function

        Private Shared Function PrepareDictionary() As XPDictionary
            Dim dict = New DevExpress.Xpo.Metadata.ReflectionDictionary()
            dict.GetDataStoreSchema(GetType(XpoReportGalleryStorage.ReportGalleryTable))
            Return dict
        End Function
    End Class
End Namespace
