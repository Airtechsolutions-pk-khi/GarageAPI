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
    
    public partial class BlogImages_Junc
    {
        public int BlogImageID { get; set; }
        public Nullable<int> BlogID { get; set; }
        public string Image { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual Blog Blog { get; set; }
    }
}