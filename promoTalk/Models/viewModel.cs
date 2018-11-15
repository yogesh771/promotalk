using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace promoTalk.Models
{
    public class viewModel
    {
    }
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name required")]
        [Display(Name = "User Name")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "User name required")]
        [Display(Name = "Password")]
        public string password { get; set; }
    }

    [MetadataType(typeof(countryViewModel))]
    public partial class country
    {
    }
    public class countryViewModel
    {
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country name required")]
        public string countryName { get; set; }
    }


    [MetadataType(typeof(stateViewModel))]
    public partial class state
    {
    }
    public class stateViewModel
    {
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country required")]
        public string countryID { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State required")]
        public string StateName { get; set; }

    }
    [MetadataType(typeof(supplierViewModel))]
    public partial class supplier
    {
        //[Display(Name = "Country")]
        
        //public string countryID { get; set; }
    }
    public class supplierViewModel
    {

        [Display(Name = "Supplier Name")]
        [Required(ErrorMessage = "Supplier name required")]
        public string supplierName { get; set; }

        //[Display(Name = "City ")]
        //[Required(ErrorMessage = "City name required")]
        //public string City { get; set; }

        //[Display(Name = "State")]
        //[Required(ErrorMessage = "State required")]
        //public int StateID { get; set; }
        [Display(Name = "Web address")]
        [Required(ErrorMessage = "Web address required")]
        public string address { get; set; }

        //[Display(Name = "Phone")]
        //[Required(ErrorMessage = "Phone required")]
        //public string phone { get; set; }
    }
    [MetadataType(typeof(productCategoryViewModel))]
    public partial class productCategory
    {

    }
    public class productCategoryViewModel
    {

        [Display(Name = "Product Category")]
        [Required(ErrorMessage = "Product category required")]
        public string productCategory1 { get; set; }
    }
    [MetadataType(typeof(productCatalogViewModel))]
    public partial class productCatalog
    {

    }
    public class productCatalogViewModel
    {
        [Display(Name = "Supplier")]
        [Required(ErrorMessage = "Supplier required")]
        public int supplierID { get; set; }     

        [Display(Name = "Product Title")]
        [Required(ErrorMessage = "Product title required")]
        public string productTitle { get; set; }

        
        [Required(ErrorMessage = "Slug url required")]
        [Display(Name = "Slug url")]
        public string slugURL { get; set; }

        [Display(Name = "Is premium")]
        public bool isPremeum { get; set; }

        [Display(Name = "Is Flyer")]
        public bool isOffer { get; set; }

       
        [Display(Name = "Seo Meta Keyword")]
        public string seo_metaKeyword { get; set; }

      
        [Display(Name = "OG Image")]
        public string seo_OGImage { get; set; }

      
        [Display(Name = "OG Desription")]
        public string seo_OGDesription { get; set; }

       
        [Display(Name = "OG Title")]
        public string seo_OGTitle { get; set; }

        [Required(ErrorMessage = "Provider url required")]
        [Display(Name = "Provider url")]
        public string offerProviderURl { get; set; }
    }


    [MetadataType(typeof(marketingViewModel))]
    public partial class marketing
    {

    }

    [MetadataType(typeof(marketingViewModel))]
    public partial class news
    {      
    }

    [MetadataType(typeof(marketingViewModel))]
    public partial class technology
    {

    }
    public class marketingViewModel{

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Product title required")]
        public string title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description required")]
        [AllowHtml]
        public string description { get; set; }

        [Display(Name = "Seo Meta Keyword")]       
        public string seo_metaKeyword { get; set; }

        [Display(Name = "Seo Og Image")] 
        public string seo_OGImage { get; set; }

   
        [Display(Name = "OG Desription")]
        public string seo_OGDesription { get; set; }

       
        [Display(Name = "OG Title")]
        public string seo_OGTitle { get; set; }

        [Display(Name = "Created Date")]      
        public Nullable<System.DateTime> createdDate { get; set; }

        [AllowHtml]
        [Display(Name = "Preview Text")]
        public string shortDescription { get; set; }

        [Required(ErrorMessage = "Slug url required")]
        [Display(Name = "Slug url")]
        public string slugURL { get; set; }
    }
    [MetadataType(typeof(eventsViewModel))]
    public partial class tbl_events { }

    public class eventsViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Event name required")]
        [StringLength(500,ErrorMessage ="Event name should be min 5 and max 500 charecters",MinimumLength =5)]
        public string name { get; set; }

        [Required(ErrorMessage = "Slug url required")]
        [Display(Name = "Slug url")]
        public string slugURL { get; set; }

        [AllowHtml]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Venue")]
        [Required(ErrorMessage = "Event vanue required")]
        [StringLength(300, ErrorMessage = "Event vanue should be min 10 and max 300 charecters", MinimumLength = 10)]
        public string venue { get; set; }

      
        [Required(ErrorMessage = "Event from date time required")]
        [Display(Name = "From Date")]
        public DateTime dateTime { get; set; }

        [Required(ErrorMessage = "Event till date time required")]
        [Display(Name = "To Date")]
        public DateTime TillDateime { get; set; }

        [Display(Name = "Created Date")]
        public DateTime createdDate { get; set; }

        [Display(Name = "Host Name")]
        [Required(ErrorMessage = "Hoster name required")]       
        public string Hoster { get; set; }

        [Display(Name = "Industrial Event or Expo")]
        public bool industrialEvent { get; set; }
        [Display(Name = "Association Event or Expo")]
        public bool associationEvent { get; set; }
        [Display(Name = "Printers Expo")]
        public bool printerEvent { get; set; }
        [Display(Name = "Embroiders Expo")]
        public bool emroidersEvent { get; set; }

        [Display(Name = "Decorators Event")]
        public bool decorators { get; set; }
        
        [Display(Name = "Source Url")]
        public bool sourceUrl { get; set; }


        [Display(Name = "Location")]
        [Required(ErrorMessage = "Event location required")]
        [StringLength(300, ErrorMessage = "Event location should be min 10 and max 300 charecters", MinimumLength = 10)]
        public string Location { get; set; }

        
        [Display(Name = "Seo Meta Keyword")]
        public string seo_metaKeyword { get; set; }


       
        [Display(Name = "OG Desription")]
        public string seo_OGDesription { get; set; }

      
        [Display(Name = "OG Title")]
        public string seo_OGTitle { get; set; }

        [Display(Name = "Event Website")]
        public string EventWebsite { get; set; }
    }


    [MetadataType(typeof(serviceViewModel))]
    public partial class service { }

    public class serviceViewModel
    {
        [Required(ErrorMessage = "Service Name required")]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Created Date")]
        public System.DateTime createdDate { get; set; }
    }


    [MetadataType(typeof(buisinessInformationViewModel))]
    public partial class buisinessInformation {
        public string servicesIDs { get; set; }
    }

    public class buisinessInformationViewModel
    {
        [Required(ErrorMessage = "Business Name required")]
        [Display(Name = "Business Name")]
        public string bname { get; set; }

        [Required(ErrorMessage = "Slug url required")]
        [Display(Name = "Slug url")]
        public string slugURL { get; set; }

        [Required(ErrorMessage = "Description required")]
        [Display(Name = "Description")]
        [AllowHtml]
        public string description { get; set; }

        [Required(ErrorMessage = "Website Url required")]
        [Display(Name = "Website")]
        public string website { get; set; }

        [Required(ErrorMessage = "Business Contact required")]
        [Display(Name = "Business Contact")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Phone required")]
        [Display(Name = "Phone")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Email required")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string email { get; set; }
  
        [Display(Name = "Screen Printer")]
        public bool isScreenPrinter { get; set; }

        [Display(Name = "Embroiders")]
        public bool isEmbroiders { get; set; }

        [Display(Name = "Created Date")]
        public System.DateTime createdDate { get; set; }

        [Display(Name = "Active")]
        public bool isActive { get; set; }

        [Display(Name = "Premium Member")]
        public bool isPremeum { get; set; }

        [Display(Name = "Logo")]
        public string logo { get; set; }

        [Required(ErrorMessage = "Location required")]
        [Display(Name = "Address")]
        public string  location { get; set; }

        [Required(ErrorMessage = "Zip Code required")]
        [Display(Name = "Zip Code")]
        public String zipCode { get; set; }

        [Required(ErrorMessage = "City required")]
        [Display(Name = "City")]
        public String City { get; set; }

        [Required(ErrorMessage = "State required")]
        [Display(Name = "State")]
        public String StateID { get; set; }

        [Required(ErrorMessage = "Country required")]
        [Display(Name = "Country")]
        public String countryID { get; set; }

        [Display(Name = "Website")]
        public string seo_metaKeyword { get; set; }

       
        [Display(Name = "OG Image")]
        public string seo_OGImage { get; set; }

      
        [Display(Name = "OG Desription")]
        public string seo_OGDesription { get; set; }

        [Display(Name = "OG Title")]        
        public string seo_OGTitle { get; set; }


    }
    public class selectedService
    {
        public int serviceID { get; set; }
        public bool ischecked { get; set; }
        public string ServiceName { get; set; }
    }
    [MetadataType(typeof(tbl_customOrderViewModel))]
    public partial class tbl_customOrder {
        public string category { get; set; }
    }

    public class tbl_customOrderViewModel
    {
        [Required(ErrorMessage = "Name required")]
        [Display(Name = "Contact Name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Company Name required")]
        [Display(Name = "Company Name")]
        public string companyName { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Comments required")]
        [Display(Name = "Instructions")]
        public string comments { get; set; }

        [Required(ErrorMessage = "Email Id required")]
        [Display(Name = "Email Address")]
        public string email { get; set; }

        [Required(ErrorMessage = "Title required")]
        [Display(Name = "Title")]
        public string other1 { get; set; }


        [Display(Name = "Titleurl")]
        public string other2 { get; set; }

        [Required(ErrorMessage = "Phone number required")]
        [Display(Name = "Phone")]
        [PhoneAttribute]
        public string phone { get; set; }
        [Url]
        [Required(ErrorMessage = "Company url required")]
        [Display(Name = "Company URL")]
        public string companyURl { get; set; }


        [Display(Name = "Company Logo")]
        public string logo { get; set; }


    }
    public class catalogDescription
    {
        public productCatalog oproductCatalog { get; set; }
        public IEnumerable<productCatalog> productCatalogList { get; set; }
    }

}