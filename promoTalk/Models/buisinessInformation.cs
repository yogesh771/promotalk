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
    
    public partial class buisinessInformation
    {
        public int buisinessID { get; set; }
        public string bname { get; set; }
        public string description { get; set; }
        public string website { get; set; }
        public string ContactPerson { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool isScreenPrinter { get; set; }
        public bool isEmbroiders { get; set; }
        public System.DateTime createdDate { get; set; }
        public bool isActive { get; set; }
        public string logo { get; set; }
        public string location { get; set; }
        public string zipCode { get; set; }
        public bool isPremeum { get; set; }
        public string seo_metaKeyword { get; set; }
        public string seo_OGImage { get; set; }
        public string seo_OGDesription { get; set; }
        public string seo_OGTitle { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public int countryID { get; set; }
        public string slugURL { get; set; }
    
        public virtual country country { get; set; }
        public virtual state state { get; set; }
    }
}
