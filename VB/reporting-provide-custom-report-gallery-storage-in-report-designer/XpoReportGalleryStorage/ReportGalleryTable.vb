Imports DevExpress.Xpo
Imports DevExpress.XtraReports.ReportGallery

Namespace XpoReportGalleryStorage

    Public Class ReportGalleryTable
        Inherits XPLiteObject

        Private sIDField As String

        Private displayNameField As String

        Private hashField As Integer

        Private contentHashField As Integer

        Private galleryItemType As GalleryItemType

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        <Key>
        Public Property ID As Integer

        Public Property SID As String
            Get
                Return sIDField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("SID", sIDField, value)
            End Set
        End Property

        Public Property ItemKind As String

        Public Property DisplayName As String
            Get
                Return displayNameField
            End Get

            Set(ByVal value As String)
                SetPropertyValue("DisplayName", displayNameField, value)
            End Set
        End Property

        Public Property Content As Byte()

        Public Property Hash As Integer
            Get
                Return hashField
            End Get

            Set(ByVal value As Integer)
                SetPropertyValue("Hash", hashField, value)
            End Set
        End Property

        Public Property ContentHash As Integer
            Get
                Return contentHashField
            End Get

            Set(ByVal value As Integer)
                SetPropertyValue("ContentHash", contentHashField, value)
            End Set
        End Property

        Public Property ItemType As GalleryItemType
            Get
                Return galleryItemType
            End Get

            Set(ByVal value As GalleryItemType)
                SetPropertyValue("ItemType", galleryItemType, value)
            End Set
        End Property
    End Class
End Namespace
