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
    
    public partial class AttendedCollege
    {
        public int CollegeID { get; set; }
        public int StudentID { get; set; }
        public bool CurrentlyAttending { get; set; }
        public int LastYearAttended { get; set; }
        public double GPA { get; set; }
    
        public virtual College College { get; set; }
        public virtual Student Student { get; set; }
    }
}
