using DevExpress.Xpo;
using DevExpress.XtraReports.ReportGallery;

namespace XpoReportGalleryStorage {
    public class ReportGalleryTable : XPLiteObject {
        string sID;
        string displayName;
        int hash;
        int contentHash;
        GalleryItemType galleryItemType;

        public ReportGalleryTable(Session session) : base(session) {
        }

        [Key]
        public int ID { get; set; }

        public string SID {
            get { return sID; }
            set { SetPropertyValue("SID", ref sID, value); }
        }

        public string ItemKind { get; set; }

        public string DisplayName {
            get { return displayName; }
            set { SetPropertyValue("DisplayName", ref displayName, value); }
        }

        public byte[] Content { get; set; }

        public int Hash {
            get { return hash; }
            set { SetPropertyValue("Hash", ref hash, value); }
        }

        public int ContentHash {
            get { return contentHash; }
            set { SetPropertyValue("ContentHash", ref contentHash, value); }
        }

        public GalleryItemType ItemType {
            get { return galleryItemType; }
            set { SetPropertyValue("ItemType", ref galleryItemType, value); }
        }

    }
}
