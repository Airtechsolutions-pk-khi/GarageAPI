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
    
    public partial class Notification
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<bool> IsRead { get; set; }
    }
}
