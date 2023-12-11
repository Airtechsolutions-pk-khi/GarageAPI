//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.DBEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.DiscountItems = new HashSet<DiscountItem>();
            this.inv_BillDetail = new HashSet<inv_BillDetail>();
            this.inv_PurchaseOrderDetail = new HashSet<inv_PurchaseOrderDetail>();
            this.inv_ReconciliationDetail = new HashSet<inv_ReconciliationDetail>();
            this.inv_Stock = new HashSet<inv_Stock>();
            this.inv_StockIssueDetail = new HashSet<inv_StockIssueDetail>();
            this.inv_StockRequestDetail = new HashSet<inv_StockRequestDetail>();
            this.Inventories = new HashSet<Inventory>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.OrderDetailPackages = new HashSet<OrderDetailPackage>();
            this.PackageDetails = new HashSet<PackageDetail>();
        }
    
        public int ItemID { get; set; }
        public int RowID { get; set; }
        public int SubCatID { get; set; }
        public string Name { get; set; }
        public string NameOnReceipt { get; set; }
        public string Description { get; set; }
        public string ItemImage { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<bool> SortByAlpha { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> Price { get; set; }
        public string ItemType { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<bool> FeaturedItem { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string ItemTypeValue { get; set; }
        public Nullable<bool> IsInventoryItem { get; set; }
        public Nullable<bool> IsOpenItem { get; set; }
        public Nullable<double> MinOpenPrice { get; set; }
        public Nullable<int> OdooProductID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscountItem> DiscountItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_BillDetail> inv_BillDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_PurchaseOrderDetail> inv_PurchaseOrderDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_ReconciliationDetail> inv_ReconciliationDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_Stock> inv_Stock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_StockIssueDetail> inv_StockIssueDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inv_StockRequestDetail> inv_StockRequestDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual Status Status { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetailPackage> OrderDetailPackages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PackageDetail> PackageDetails { get; set; }
    }
}
