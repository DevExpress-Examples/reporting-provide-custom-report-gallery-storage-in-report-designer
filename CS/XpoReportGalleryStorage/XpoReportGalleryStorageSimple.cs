using System.Linq;
using DevExpress.Xpo;
using DevExpress.XtraReports.ReportGallery;

namespace XpoReportGalleryStorage {
    public class XpoReportGalleryStorageSimple : XpoReportGalleryStorageBase {

        // Enable synchronization with a remote storage.
        public override bool EnableSynchronization => true;

        public override void SaveGallery(Gallery gallery) {
            using(UnitOfWork unitOfWork = new UnitOfWork(DataLayer)) {
                unitOfWork.BeginTransaction();
                var deletedItems = unitOfWork.Query<ReportGalleryTable>().Select(a => a);
                foreach(var deletes in deletedItems) {
                    deletes.Delete();
                }
                unitOfWork.CommitTransaction();
                unitOfWork.BeginTransaction();
                foreach(var newItem in gallery.Items)
                    MapFromGalleryItem(newItem, unitOfWork);
                unitOfWork.CommitTransaction();
            }
        }

        public override bool TryLoadGallery(out Gallery gallery) {
            using(UnitOfWork unitOfWork = new UnitOfWork(DataLayer)) {
                gallery = new Gallery();
                var remoteItems = unitOfWork.Query<ReportGalleryTable>().Select(a => MapFromTableRow(a)).ToList();
                if(!remoteItems.Any())
                    return false;
                gallery.Items.AddRange(remoteItems);
                return true;
            }
        }
    }
}
