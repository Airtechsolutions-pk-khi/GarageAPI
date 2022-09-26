//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.DBEntities2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            this.Bays = new HashSet<Bay>();
            this.CarNotes = new HashSet<CarNote>();
            this.Categories = new HashSet<Category>();
            this.Discounts = new HashSet<Discount>();
            this.inv_Bill = new HashSet<inv_Bill>();
            this.inv_PurchaseOrder = new HashSet<inv_PurchaseOrder>();
            this.inv_Reconciliation = new HashSet<inv_Reconciliation>();
            this.inv_StockRequest = new HashSet<inv_StockRequest>();
            this.Inventories = new HashSet<Inventory>();
            this.LocationAmenitiesJuncs = new HashSet<LocationAmenitiesJunc>();
            this.LocationServiceJuncs = new HashSet<LocationServiceJunc>();
            this.Modifiers = new HashSet<Modifier>();
            this.OrderCheckouts = new HashSet<OrderCheckout>();
            this.Orders = new HashSet<Order>();
            this.Packages = new HashSet<Package>();
            this.Receipts = new HashSet<Receipt>();
            this.SalesItems = new HashSet<SalesItem>();
            this.SalesSettings = new HashSet<SalesSetting>();
            this.SessionInfoes = new HashSet<SessionInfo>();
            this.Stores = new HashSet<Store>();
            this.SubUsers = new HashSet<SubUser>();
        }
    
        public int LocationID { get; set; }
        public int RowID { get; set; }
        public string Name { get; set; }
        public string Descripiton { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public Nullable<int> TimeZoneID { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string CountryID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<System.TimeSpan> Open_Time { get; set; }
        public Nullable<System.TimeSpan> Close_Time { get; set; }
        public int UserID { get; set; }
        public Nullable<int> LicenseID { get; set; }
        public Nullable<bool> DeliveryServices { get; set; }
        public Nullable<double> DeliveryCharges { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<double> MinOrderAmount { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string CompanyCode { get; set; }
        public Nullable<int> LandmarkID { get; set; }
        public string GMapLink { get; set; }
        public string ImageURL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bay> Bays { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarNote> CarNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Discount> Discounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_Bill> inv_Bill { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_PurchaseOrder> inv_PurchaseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_Reconciliation> inv_Reconciliation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_StockRequest> inv_StockRequest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual License License { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationAmenitiesJunc> LocationAmenitiesJuncs { get; set; }
        public virtual Location Locations1 { get; set; }
        public virtual Location Location1 { get; set; }
        public virtual Status Status { get; set; }
        public virtual TimeZone TimeZone { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationServiceJunc> LocationServiceJuncs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Modifier> Modifiers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCheckout> OrderCheckouts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Package> Packages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> Receipts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesItem> SalesItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesSetting> SalesSettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SessionInfo> SessionInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubUser> SubUsers { get; set; }
    }
}
