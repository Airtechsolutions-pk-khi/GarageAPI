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
    
    public partial class inv_ReconciliationDetail
    {
        public int ReconciliationDetailID { get; set; }
        public Nullable<int> ReconciliationID { get; set; }
        public int ItemID { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Reason { get; set; }
    
        public virtual inv_Reconciliation inv_Reconciliation { get; set; }
        public virtual Item Item { get; set; }
    }
}