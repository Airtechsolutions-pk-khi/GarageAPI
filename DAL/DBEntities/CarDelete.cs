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
    
    public partial class CarDelete
    {
        public int RequestID { get; set; }
        public Nullable<int> CarID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string RequestFrom { get; set; }
        public Nullable<int> RequestStatus { get; set; }
        public string Reason { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual Customer Customer { get; set; }
    }
}