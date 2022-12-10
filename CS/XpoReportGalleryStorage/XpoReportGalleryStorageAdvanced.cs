using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.XtraReports.ReportGallery;

namespace XpoReportGalleryStorage {
    public class XpoReportGalleryStorageAdvanced : XpoReportGalleryStorageBase {

        // Enable synchronization with a remote storage.
        public override bool EnableSynchronization => true;

        // This method's default implementation loads all gallery items from the storage and compares them with all local items.
        // In this example, this method is overridden to get gallery changes only.  
        protected override GalleryChanges GetChanges(IEnumerable<GalleryItem> galleryItems) {
            var remoteHashList = LoadHashList(); 
            var localHashList = galleryItems.Select(a => a.GetHashCode());
			var hashListToDelete = localHashList.Except(remoteHashList).ToList(); 
            var hashListToAdd = remoteHashList.Except(localHashList).ToList();
			var newItems = LoadNewItems(hashListToAdd).ToList();
            var deletedItems = hashListToDelete.Join(galleryItems, hash => hash, localGallery => localGallery.GetHashCode(), (innerResult, outerResult) => outerResult).ToList();
            return new GalleryChanges() { NewItems = newItems, OldItems = deletedItems };
        }

		// The SaveGallery method overwrites the entire gallery.
		// This method is internally called in the SaveGalleryChanges method's default implementation.
		// If you do not override the SaveGalleryChanges method, uncomment the lines below.
		public override void SaveGallery(Gallery gallery) {
            //using(UnitOfWork unitOfWork = new UnitOfWork(DataLayer)) {
            //    unitOfWork.BeginTransaction();
            //    var deletedItems = unitOfWork.Query<ReportGalleryTable>().Select(a => a);
            //    foreach(var deletes in deletedItems) {
            //        deletes.Delete();
            //    }
            //    unitOfWork.CommitTransaction();
            //    unitOfWork.BeginTransaction();
            //    foreach(var newItem in gallery.Items)
            //        MapFromGalleryItem(newItem, unitOfWork);
            //    unitOfWork.CommitTransaction();
            //}
        }

		// Override the SaveGalleryChanges method to save gallery changes only (not the entire gallery).
		// If you do not override this method, it internally calls the SaveGallery method.
		public override void SaveGalleryChanges(Gallery gallery, IEnumerable<GalleryItem> changedItems, GalleryChangeType changeType) {
            using(var unitOfWork = new UnitOfWork(DataLayer)) {
                unitOfWork.BeginTransaction();
                var ids = changedItems.Select(a => int.Parse(a.ID)).ToList();
                var queryResult = unitOfWork.QueryInTransaction<ReportGalleryTable>().Where(a => ids.Contains(a.ID)); 
				foreach (var item in changedItems) {
                    var changedItem = queryResult.FirstOrDefault(a => a.ID == int.Parse(item.ID));
                    if(changeType == GalleryChangeType.Delete)
                        changedItem.Delete();
                    if(changeType == GalleryChangeType.Update) {
                        changedItem.SID = item.SID;
                        changedItem.DisplayName = item.DisplayName;
                        changedItem.Hash = item.GetHashCode();
                        changedItem.ContentHash = item.ContentHash;
                        changedItem.ItemType = item.ItemType;
                    }
                    if(changeType == GalleryChangeType.Add)
                        MapFromGalleryItem(item, unitOfWork);
                }
                try {
                    unitOfWork.CommitTransaction(); 
                } catch {
                    unitOfWork.RollbackTransaction();
                }
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
        
        IEnumerable<int> LoadHashList() {
            using(UnitOfWork unitOfWork = new UnitOfWork(DataLayer)) {
                var remoteHashList = unitOfWork.Query<ReportGalleryTable>().Select(a => a.Hash).ToList();
                return remoteHashList;
            }
        }

        IEnumerable<GalleryItem> LoadNewItems(IEnumerable<int> newItemsHashKeys) {
            using(UnitOfWork unitOfWork = new UnitOfWork(DataLayer)) {
                var remoteItems = unitOfWork.Query<ReportGalleryTable>().Where(a => newItemsHashKeys.Contains(a.Hash)).Select(a => MapFromTableRow(a)).ToList();
                return remoteItems;
            }
        }        
    }
}
