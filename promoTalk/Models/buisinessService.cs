//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace promoTalk.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class buisinessService
    {
        public int buisinessServicesID { get; set; }
        public int buisinessID { get; set; }
        public int serviceID { get; set; }
    
        public virtual service service { get; set; }
    }
}
