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
    
    public partial class PackageDetail
    {
        public int PackageDetailID { get; set; }
        public Nullable<int> PackageID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Cost { get; set; }
        public string DiscoutType { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> StatusID { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Package Package { get; set; }
    }
}