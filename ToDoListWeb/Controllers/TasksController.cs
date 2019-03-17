using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoListWeb.DAL;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    public class TasksController : Controller
    {
        private ToDoListContext db = new ToDoListContext();

        // GET: Tasks
        public ActionResult Index(int? page, bool? deletePage = false)
        {
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            int count = 0;

            var myTasks = db.Tasks.ToArray();
            var maxPages = (myTasks.Count() / (pageSize + 1))+1;

            while (page == null && count< myTasks.Count() && myTasks[count].isComplete)
            {
                count++;
            }
            if (count >= pageSize)
            {
                if ((bool)deletePage)
                {
                    db.Tasks.RemoveRange(myTasks.ToList().GetRange(0, 25));
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                pageNumber = (count > maxPages) ? maxPages : (count / pageSize) + 1;
            }

            return View(db.Tasks.OrderBy(t => t.ID).ToPagedList(pageNumber, pageSize));
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,isComplete")] Task task)
        {
            if (ModelState.IsValid && task.Name != null)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            if (task.isComplete)
            {
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,isComplete")] Task task)
        {
            if (ModelState.IsValid)
            {
                if (task.isComplete)
                {
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    task.isComplete = true;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    task.isComplete = false;
                    db.Tasks.Add(task);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();

            return RedirectToAction("Index");
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
