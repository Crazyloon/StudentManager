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
    
    public partial class CourseTypeLookUp
    {
        public CourseTypeLookUp()
        {
            this.Courses = new HashSet<Cours>();
        }
    
        public int CourseTypeID { get; set; }
        public string CourseType { get; set; }
    
        public virtual ICollection<Cours> Courses { get; set; }
    }
}
