using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraReports.Extensions;
using DevExpress.XtraReports.ReportGallery;
using System;
using System.IO;

namespace XpoReportGalleryStorage {
    public abstract class XpoReportGalleryStorageBase : ReportGalleryExtension, IDisposable {

        ThreadSafeDataLayer dataLayer;

        IDisposable[] disposableObjects;

        protected ThreadSafeDataLayer DataLayer {
            get { return dataLayer ?? (dataLayer = CreateDataLayer()); }
        }

        protected virtual string ConnectionString { get; } = AccessConnectionProvider.GetConnectionString(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ReportGallery.mdb"));

        ThreadSafeDataLayer CreateDataLayer() {
            var dictionary = PrepareDictionary();
            using(var updateDataLayer = XpoDefault.GetDataLayer(ConnectionString, dictionary, AutoCreateOption.DatabaseAndSchema)) {
                updateDataLayer.UpdateSchema(false, dictionary.CollectClassInfos(typeof(ReportGalleryTable)));
                using(var unitOfWork = new UnitOfWork(updateDataLayer))
                    unitOfWork.CreateObjectTypeRecords(typeof(ReportGalleryTable));
            }
            var dataStore = XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption.SchemaAlreadyExists, out disposableObjects);
            return new ThreadSafeDataLayer(dictionary, dataStore);
        }

        public void Dispose() {
            if(dataLayer != null) {
                dataLayer.Dispose();
                dataLayer = null;
            }
            if(disposableObjects != null) {
                foreach(var obj in disposableObjects)
                    obj.Dispose();
                disposableObjects = null;
            }
        }

        protected GalleryItem MapFromTableRow(ReportGalleryTable row) {
            var galleryItem = new GalleryItem() {
                Content = row.Content,
                ContentHash = row.ContentHash,
                DisplayName = row.DisplayName,
                ID = row.ID.ToString(),
                ItemKind = row.ItemKind,
                SID = row.SID,
                ItemType = row.ItemType,               
            };
            return galleryItem;
        }

        protected ReportGalleryTable MapFromGalleryItem(GalleryItem galleryItem, Session session) {
            var table = new ReportGalleryTable(session) {
                ID = int.Parse(galleryItem.ID),
                SID = galleryItem.SID,
                ItemKind = galleryItem.ItemKind,
                DisplayName = galleryItem.DisplayName,
                Content = galleryItem.Content,
                Hash = galleryItem.GetHashCode(),
                ContentHash = galleryItem.ContentHash,
                ItemType = galleryItem.ItemType,
            };
            return table;
        }

        static XPDictionary PrepareDictionary() {
            var dict = new ReflectionDictionary();
            dict.GetDataStoreSchema(typeof(ReportGalleryTable));
            return dict;
        }
    }
}
