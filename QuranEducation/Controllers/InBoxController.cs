using QuranEducation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Controllers
{
    public class InBoxController : MyController
    {
        private ApplicationDbContext _context;
        DateTime _currentDate = DateTime.Now;
        public InBoxController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: InstructorProfile
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Index()
        {
            var inMessages = _context.Inbox.Where(m => m.AUserName == User.Identity.Name).OrderByDescending(m=>m.Id).ToList();
            return View(inMessages);
        }
        public ActionResult Remove(int Id)
        {
            var inMessages = _context.Inbox.FirstOrDefault(m => m.Id==Id);
            _context.Inbox.Remove(inMessages);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Card_Feture/Delete/5
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inbox  inbox = _context.Inbox.Find(Id);
            if (inbox == null)
            {
                return HttpNotFound();
            }
            return View(inbox);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            Inbox inbox = _context.Inbox.Find(Id);
            _context.Inbox.Remove(inbox);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}