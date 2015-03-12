using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositoryWithCaching.Models;

namespace RepositoryWithCaching.Controllers
{
    public class CommentsController : Controller
    {
        private MyDataRepository dr = new MyDataRepository();
        
        //
        // GET: /Comments/

        public ActionResult Index()
        {
            var comments = dr.RetrieveComments().Include("Blog").OrderBy(c => c.CommentID )  .Skip(2).Take (15);//c => c.Blog
            return View(comments.ToList());
        }

        //
        // GET: /Comments/Details/5

        public ActionResult Details(int id = 0)
        {
            Comment comment = dr.RetrieveComment(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // GET: /Comments/Create

        public ActionResult Create()
        {
            ViewBag.BlogID = new SelectList(dr.RetrieveBlogs() , "BlogID", "Blog Post");
            return View();
        }

        //
        // POST: /Comments/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                dr.AddComment (comment);
                dr.Save();
                return RedirectToAction("Index");
            }

            ViewBag.BlogID = new SelectList(dr.RetrieveBlogs(), "BlogID", "Title", comment.BlogID);
            return View(comment);
        }

        //
        // GET: /Comments/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Comment comment = dr.RetrieveComment(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogID = new SelectList(dr.RetrieveBlogs(), "BlogID", "Title", comment.BlogID);
            return View(comment);
        }

        //
        // POST: /Comments/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                dr.UpdateComment(comment);
                dr.Save();
                return RedirectToAction("Index");
            }
            ViewBag.BlogID = new SelectList(dr.RetrieveBlogs(), "BlogID", "Title", comment.BlogID);
            return View(comment);
        }

        //
        // GET: /Comments/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Comment comment = dr.RetrieveComment(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // POST: /Comments/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = dr.RetrieveComment(id);
            dr.DeleteComment(comment);
            dr.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //dr.Dispose();
            base.Dispose(disposing);
        }
    }
}
