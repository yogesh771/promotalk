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
    
    public partial class tbl_events
    {
        public int eventID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string venue { get; set; }
        public System.DateTime dateTime { get; set; }
        public System.DateTime createdDate { get; set; }
        public System.DateTime TillDateime { get; set; }
        public string Hoster { get; set; }
        public bool industrialEvent { get; set; }
        public bool associationEvent { get; set; }
        public bool printerEvent { get; set; }
        public bool emroidersEvent { get; set; }
        public string sourceUrl { get; set; }
        public bool decorators { get; set; }
        public string featuredImag { get; set; }
        public string Location { get; set; }
        public string seo_metaKeyword { get; set; }
        public string seo_OGDesription { get; set; }
        public string seo_OGTitle { get; set; }
        public string EventWebsite { get; set; }
        public string slugURL { get; set; }
    }
}