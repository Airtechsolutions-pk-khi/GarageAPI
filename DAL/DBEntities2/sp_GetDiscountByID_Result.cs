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
    
    public partial class sp_GetDiscountByID_Result
    {
        public int DiscountID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string ArabicDescription { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Image { get; set; }
        public string ArabicImage { get; set; }
        public string ThumbnailImage { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Locations { get; set; }
    }
}