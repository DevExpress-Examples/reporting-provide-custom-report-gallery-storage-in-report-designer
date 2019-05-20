Imports DevExpress.Xpo
Imports DevExpress.XtraReports.ReportGallery

Namespace XpoReportGalleryStorage
    Public Class ReportGalleryTable
        Inherits XPLiteObject


        Private sID_Renamed As String

        Private displayName_Renamed As String

        Private hash_Renamed As Integer

        Private contentHash_Renamed As Integer
        Private galleryItemType As GalleryItemType

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        <Key> _
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
