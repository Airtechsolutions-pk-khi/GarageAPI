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
    
    public partial class sp_GetCarOrderDetail_App_Result
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> PackageID { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> ItemCost { get; set; }
        public Nullable<double> RefundQty { get; set; }
        public Nullable<double> TotalCost { get; set; }
        public Nullable<double> LOYALTYPoints { get; set; }
        public Nullable<bool> isComplementory { get; set; }
        public string SessionID { get; set; }
        public string OrderMode { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<double> RefundAmount { get; set; }
        public Nullable<double> DiscountAmount { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> OrderPunchDt { get; set; }
        public Nullable<int> CarID { get; set; }
        public string Name { get; set; }
        public string NameOnReceipt { get; set; }
        public Nullable<double> CheckLiters { get; set; }
    }
}