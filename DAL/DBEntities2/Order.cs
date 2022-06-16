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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.CarNotes = new HashSet<CarNote>();
            this.OrderCheckouts = new HashSet<OrderCheckout>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.OrderInspectionMappings = new HashSet<OrderInspectionMapping>();
            this.OrdersChecklists = new HashSet<OrdersChecklist>();
            this.PaymentDetails = new HashSet<PaymentDetail>();
        }
    
        public int OrderID { get; set; }
        public Nullable<int> TransactionNo { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<int> OrderTakerID { get; set; }
        public Nullable<int> CarID { get; set; }
        public Nullable<int> BayID { get; set; }
        public string OrderType { get; set; }
        public Nullable<double> Points { get; set; }
        public string OrderMode { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> OrderPunchDt { get; set; }
        public Nullable<System.DateTime> OrderCreatedDT { get; set; }
        public string SessionID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }
        public int LocationID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> SubUserID { get; set; }
        public Nullable<double> CheckLiters { get; set; }
        public Nullable<double> NextKM { get; set; }
    
        public virtual Bay Bay { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarNote> CarNotes { get; set; }
        public virtual Car Car { get; set; }
        public virtual Location Location { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCheckout> OrderCheckouts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderInspectionMapping> OrderInspectionMappings { get; set; }
        public virtual SubUser SubUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersChecklist> OrdersChecklists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}