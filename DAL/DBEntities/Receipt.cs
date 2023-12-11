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
    
    public partial class Receipt
    {
        public int ReceiptID { get; set; }
        public string ReceiptName { get; set; }
        public string CompanyTitle { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhones { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        public string Companytagline { get; set; }
        public string CompanyLogoURL { get; set; }
        public string Footer { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsCompanyTagline { get; set; }
        public Nullable<int> StatusID { get; set; }
        public bool IsActive { get; set; }
        public int LocationID { get; set; }
        public Nullable<bool> IsA4Spacing { get; set; }
        public string QRTagline { get; set; }
        public string QRLink { get; set; }
    
        public virtual Location Location { get; set; }
    }
}
