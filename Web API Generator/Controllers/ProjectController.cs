using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_API_Generator.Models;

namespace Web_API_Generator.Controllers
{
    public class ProjectController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var ListProject = db.projects.ToList();
            return View(ListProject);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(new ProjectViewModels());
        }

        [HttpPost]
        public ActionResult Add(ProjectViewModels Project)
        {
            if (!ModelState.IsValid)
            {
                return View(Project);
            }
            try
            {
                db.projects.Add(Project);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                var excep = ex.Message;
            }
         
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Project = db.projects.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult Edit(ProjectViewModels Project)
        {
            if (!ModelState.IsValid)
            {
                return View(Project);
            }
                this.db.Entry<ProjectViewModels>(Project).State = EntityState.Modified;
                this.db.SaveChanges();
                return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var Project = db.projects.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {

            var DeletedProject = db.projects.FirstOrDefault(emp => emp.Id == id);
            this.db.projects.Attach(DeletedProject);
            this.db.projects.Remove(DeletedProject);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            var Project = db.projects.Include(x=>x.TableViewModels).FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
    }
}