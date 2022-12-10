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

'INSTANT VB NOTE: The field dataLayer was renamed since Visual Basic does not allow fields to have the same name as other class members:
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
			Dim galleryItem = New GalleryItem() With {
				.Content = row.Content,
				.ContentHash = row.ContentHash,
				.DisplayName = row.DisplayName,
				.ID = row.ID.ToString(),
				.ItemKind = row.ItemKind,
				.SID = row.SID,
				.ItemType = row.ItemType
			}
			Return galleryItem
		End Function

		Protected Function MapFromGalleryItem(ByVal galleryItem As GalleryItem, ByVal session As Session) As ReportGalleryTable
			Dim table = New ReportGalleryTable(session) With {
				.ID = Integer.Parse(galleryItem.ID),
				.SID = galleryItem.SID,
				.ItemKind = galleryItem.ItemKind,
				.DisplayName = galleryItem.DisplayName,
				.Content = galleryItem.Content,
				.Hash = galleryItem.GetHashCode(),
				.ContentHash = galleryItem.ContentHash,
				.ItemType = galleryItem.ItemType
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
