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
    
    public partial class sp_GetLocationsByID_CADMIN_Result
    {
        public int LocationID { get; set; }
        public int RowID { get; set; }
        public string Name { get; set; }
        public string Descripiton { get; set; }
        public string ArabicDescription { get; set; }
        public string Address { get; set; }
        public string ArabicAddress { get; set; }
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
        public Nullable<int> CustomerStatusID { get; set; }
        public string CompanyCode { get; set; }
        public Nullable<int> LandmarkID { get; set; }
        public string Gmaplink { get; set; }
        public string ImageURL { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        public string ArabicName { get; set; }
        public string Currency { get; set; }
        public string VATNO { get; set; }
        public Nullable<double> Tax { get; set; }
        public Nullable<bool> AllowNegativeInventory { get; set; }
        public string Amenities { get; set; }
        public string Service { get; set; }
    }
}
