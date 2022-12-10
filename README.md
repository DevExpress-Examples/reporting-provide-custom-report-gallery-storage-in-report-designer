## WinForms and WPF Report Designers - How to implement a custom Report Gallery storage 

This example demonstrates how to store Report Gallery templates in a database and share them between multiple users.

Steps to implement a custom storage:
1. Create a descendant of the abstract [ReportGalleryExtension](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension) class.
2. Set the [ReportGalleryExtension.EnableSynchronization](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.EnableSynchronization) property to **true** to enable synchronization between a shared database storage and local Report Gallery instances. This also adds the **Refresh** button to Report Gallery toolbars. End users can click this button to update Gallery content explicitly. 
3. Implement the [TryLoadGallery](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.TryLoadGallery(DevExpress.XtraReports.ReportGallery.Gallery-)) method to load the entire Report Gallery from the database storage.
4. Implement the [SaveGallery](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.SaveGallery(DevExpress.XtraReports.ReportGallery.Gallery)) or [SaveGalleryChanges](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.SaveGalleryChanges(Gallery--IEnumerable-GalleryItem---GalleryActionType)) method to save the entire Gallery or only the changes that have been made.


This example demonstrates how to implement the following storages: 

* **XpoReportGalleryStorageSimple** - saves and loads all the gallery items.

    In this storage, the [SaveGallery](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.SaveGallery(DevExpress.XtraReports.ReportGallery.Gallery)) is implemented to overwrite the entire Report Gallery. The [TryLoadGallery](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.TryLoadGallery(DevExpress.XtraReports.ReportGallery.Gallery-)) method is called when the Gallery runs and whenever a user manages gallery items. 

* **XpoReportGalleryStorageAdvanced** - saves and loads the changed items only.

    In this storage, the [SaveGalleryChanges](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.SaveGalleryChanges(Gallery--IEnumerable-GalleryItem---GalleryActionType)) method is implemented to save Report Gallery changes. The protected **GetChanges** method is overridden to compare changed items only. This method is called whenever a user manages gallery items. The  [TryLoadGallery](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.TryLoadGallery(DevExpress.XtraReports.ReportGallery.Gallery-)) method is called only when the Gallery runs.


Do the following to register the Report Gallery storage in your application:

* **WinForms**

    Call the static [ReportGalleryExtension.RegisterExtensionGlobal](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Extensions.ReportGalleryExtension.RegisterExtensionGlobal(DevExpress.XtraReports.Extensions.ReportGalleryExtension)) method and pass the storage instance as the parameter. This registers the storage in the application globally.

* **WPF**

    Create a storage instance and assign it to the Report Designer's [ReportGalleryOptions.Storage](https://docs.devexpress.com/WPF/DevExpress.Xpf.Reports.UserDesigner.ReportGalleryOptions.Storage) property.