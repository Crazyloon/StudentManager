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
    
    public partial class BAS_StatusLookup
    {
        public BAS_StatusLookup()
        {
            this.StudentDetails = new HashSet<StudentDetail>();
        }
    
        public int StatusID { get; set; }
        public string StatusType { get; set; }
    
        public virtual ICollection<StudentDetail> StudentDetails { get; set; }
    }
}