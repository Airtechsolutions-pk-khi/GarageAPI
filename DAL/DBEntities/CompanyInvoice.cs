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
    
    public partial class CompanyInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyInvoice()
        {
            this.CompanyInvoiceDetails = new HashSet<CompanyInvoiceDetail>();
        }
    
        public int CompanyInvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> SupplyDate { get; set; }
        public string TaxNo { get; set; }
        public string SellerName { get; set; }
        public string SellerAddress { get; set; }
        public string SellerVAT { get; set; }
        public string SellerContact { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerContact { get; set; }
        public string BuyerVAT { get; set; }
        public Nullable<double> TotalDiscount { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<double> TotalVAT { get; set; }
        public Nullable<double> GrandTotal { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Notes { get; set; }
        public string Terms { get; set; }
        public Nullable<int> PaymentMode { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyInvoiceDetail> CompanyInvoiceDetails { get; set; }
    }
}
