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
    
    public partial class JobTitleLookUp
    {
        public JobTitleLookUp()
        {
            this.EmploymentStatus = new HashSet<EmploymentStatu>();
        }
    
        public int JobTitleID { get; set; }
        public string JobTitle { get; set; }
    
        public virtual ICollection<EmploymentStatu> EmploymentStatus { get; set; }
    }
}
