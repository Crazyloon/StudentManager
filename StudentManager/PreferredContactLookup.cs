//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class PreferredContactLookup
    {
        public PreferredContactLookup()
        {
            this.StudentDetails = new HashSet<StudentDetail>();
        }
    
        public int ContactTypeID { get; set; }
        public string ContactMethod { get; set; }
    
        public virtual ICollection<StudentDetail> StudentDetails { get; set; }
    }
}