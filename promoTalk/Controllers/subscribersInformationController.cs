using promoTalk.Models;
using System.Linq;
using System.Web.Mvc;

namespace promoTalk.Controllers
{
    public class subscribersInformationController : Controller
    {
        // GET: subscribersInformation
        private promotalkEntities db = new promotalkEntities();
        public ActionResult Index()
        {
            return View(db.SubscriberInfoes.ToList());
        }
    }
}