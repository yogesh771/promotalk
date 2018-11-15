using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using promoTalk.Models;

namespace promoTalk.Controllers
{
    public class statesController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: states
        public async Task<ActionResult> Index()
        {
            ViewBag.countryID = new SelectList(db.countries, "countryID", "countryName");
            return View();
        }
       
       
        // GET: states/Create
        public ActionResult Create()
        {
            ViewBag.countryID = db.countries;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StateID,StateName,countryID")] state state)
        {
            if (ModelState.IsValid)
            {
                db.states.Add(state);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.countryID = new SelectList(db.countries, "countryID", "countryName", state.countryID);
            return View(state);
        }

        // GET: states/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            state state = await db.states.FindAsync(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            ViewBag.countryID = new SelectList(db.countries, "countryID", "countryName", state.countryID);
            return View(state);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StateID,StateName,countryID")] state state)
        {
            if (ModelState.IsValid)
            {
                db.Entry(state).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.countryID = new SelectList(db.countries, "countryID", "countryName", state.countryID);
            return View(state);
        }
        [HttpGet]
        public ActionResult _partialStateLIst(int countryID)
        {

            var _stateList = db.states.Where(e => e.countryID == countryID).ToList();
            return PartialView("_partialStateLIst", _stateList);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
