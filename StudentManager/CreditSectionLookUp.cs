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
    
    public partial class CreditSectionLookUp
    {
        public CreditSectionLookUp()
        {
            this.Courses = new HashSet<Cours>();
        }
    
        public int CreditSectionID { get; set; }
        public string CreditSection { get; set; }
    
        public virtual ICollection<Cours> Courses { get; set; }
    }
}
