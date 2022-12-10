Imports DevExpress.Xpo
Imports DevExpress.XtraReports.ReportGallery

Namespace XpoReportGalleryStorage
	Public Class ReportGalleryTable
		Inherits XPLiteObject

'INSTANT VB NOTE: The field sID was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private sID_Renamed As String
'INSTANT VB NOTE: The field displayName was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private displayName_Renamed As String
'INSTANT VB NOTE: The field hash was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private hash_Renamed As Integer
'INSTANT VB NOTE: The field contentHash was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private contentHash_Renamed As Integer
		Private galleryItemType As GalleryItemType

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		<Key>
		Public Property ID() As Integer

		Public Property SID() As String
			Get
				Return sID_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("SID", sID_Renamed, value)
			End Set
		End Property

		Public Property ItemKind() As String

		Public Property DisplayName() As String
			Get
				Return displayName_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("DisplayName", displayName_Renamed, value)
			End Set
		End Property

		Public Property Content() As Byte()

		Public Property Hash() As Integer
			Get
				Return hash_Renamed
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("Hash", hash_Renamed, value)
			End Set
		End Property

		Public Property ContentHash() As Integer
			Get
				Return contentHash_Renamed
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("ContentHash", contentHash_Renamed, value)
			End Set
		End Property

		Public Property ItemType() As GalleryItemType
			Get
				Return galleryItemType
			End Get
			Set(ByVal value As GalleryItemType)
				SetPropertyValue("ItemType", galleryItemType, value)
			End Set
		End Property

	End Class
End Namespace
