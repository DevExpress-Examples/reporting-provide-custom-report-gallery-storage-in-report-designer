Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata
Imports DevExpress.XtraReports.Extensions
Imports DevExpress.XtraReports.ReportGallery
Imports System
Imports System.IO

Namespace XpoReportGalleryStorage
    Public MustInherit Class XpoReportGalleryStorageBase
        Inherits ReportGalleryExtension
        Implements IDisposable


        Private dataLayer_Renamed As ThreadSafeDataLayer

        Private disposableObjects() As IDisposable

        Protected ReadOnly Property DataLayer() As ThreadSafeDataLayer
            Get
                If dataLayer_Renamed IsNot Nothing Then
                    Return dataLayer_Renamed
                Else
                    dataLayer_Renamed = CreateDataLayer()
                    Return dataLayer_Renamed
                End If
            End Get
        End Property

        Protected Overridable ReadOnly Property ConnectionString() As String = AccessConnectionProvider.GetConnectionString(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\ReportGallery.mdb"))

        Private Function CreateDataLayer() As ThreadSafeDataLayer
            Dim dictionary = PrepareDictionary()
            Using updateDataLayer = XpoDefault.GetDataLayer(ConnectionString, dictionary, AutoCreateOption.DatabaseAndSchema)
                updateDataLayer.UpdateSchema(False, dictionary.CollectClassInfos(GetType(ReportGalleryTable)))
                Using unitOfWork = New UnitOfWork(updateDataLayer)
                    unitOfWork.CreateObjectTypeRecords(GetType(ReportGalleryTable))
                End Using
            End Using
            Dim dataStore = XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption.SchemaAlreadyExists, disposableObjects)
            Return New ThreadSafeDataLayer(dictionary, dataStore)
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            If dataLayer_Renamed IsNot Nothing Then
                dataLayer_Renamed.Dispose()
                dataLayer_Renamed = Nothing
            End If
            If disposableObjects IsNot Nothing Then
                For Each obj In disposableObjects
                    obj.Dispose()
                Next obj
                disposableObjects = Nothing
            End If
        End Sub

        Protected Function MapFromTableRow(ByVal row As ReportGalleryTable) As GalleryItem
            Dim galleryItem = New GalleryItem() With { _
                .Content = row.Content, _
                .ContentHash = row.ContentHash, _
                .DisplayName = row.DisplayName, _
                .ID = row.ID.ToString(), _
                .ItemKind = row.ItemKind, _
                .SID = row.SID, _
                .ItemType = row.ItemType _
            }
            Return galleryItem
        End Function

        Protected Function MapFromGalleryItem(ByVal galleryItem As GalleryItem, ByVal session As Session) As ReportGalleryTable
            Dim table = New ReportGalleryTable(session) With { _
                .ID = Integer.Parse(galleryItem.ID), _
                .SID = galleryItem.SID, _
                .ItemKind = galleryItem.ItemKind, _
                .DisplayName = galleryItem.DisplayName, _
                .Content = galleryItem.Content, _
                .Hash = galleryItem.GetHashCode(), _
                .ContentHash = galleryItem.ContentHash, _
                .ItemType = galleryItem.ItemType _
            }
            Return table
        End Function

        Private Shared Function PrepareDictionary() As XPDictionary
            Dim dict = New ReflectionDictionary()
            dict.GetDataStoreSchema(GetType(ReportGalleryTable))
            Return dict
        End Function
    End Class
End Namespace
