using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_API_Generator.Models;

namespace Web_API_Generator.Controllers
{
    public class ProjectTablesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectTables
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Models_Database(int id)
        {
            ViewBag.ProjectID = id;
            var list = db.Tables.Where(x => x.ProjectID==id && x.DataFieldType == "Database").ToList();
            return View(list);
        }
        public ActionResult Add_Models_Database(int id)
        {
            return View(new TableViewModels() { ProjectID=id, DataFieldType = "Database" });
        }
        [HttpPost]
        public ActionResult Add_Models_Database(TableViewModels TableView)
        {
            if (!ModelState.IsValid)
            {
                return View(TableView);
            }
            try
            {
                db.Tables.Add(TableView);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("Models_Database", new { id = TableView.ProjectID });
        }

        public ActionResult Edit_Models_Database(int id)
        {
            var Project = db.Tables.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult Edit_Models_Database(TableViewModels TableView)
        {
            if (!ModelState.IsValid)
            {
                return View(TableView);
            }
            try
            {
                this.db.Entry<TableViewModels>(TableView).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("Models_Database", new { id = TableView.ProjectID });
        }

        public ActionResult Delete_Models_Database(int id)
        {
            var Project = db.Tables.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [ActionName("Delete_Models_Database")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Models_DatabaseConfirm(int id)
        {
            var DeletedTables = db.Tables.FirstOrDefault(emp => emp.Id == id);
            this.db.Tables.Attach(DeletedTables);
            this.db.Tables.Remove(DeletedTables);
            this.db.SaveChanges();
            return RedirectToAction("Models_Database", new { id = DeletedTables.ProjectID });
        }


       
                          
        public ActionResult ProjectTable(int id)
        {
            var reponse = db.Tables.Include("dataFieldForeignKeyViewModels").Include("dataFieldForeignKeyViewModels.DataFieldView").Include("DataFieldViewModels").Include("DataFieldViewModels.DataAnnotationViewModels").Where(x => x.Id == id).FirstOrDefault();
            return View(reponse);
        }


        public ActionResult Add_DataFieldType(int id)
        {
            return View(new DataFieldViewModels() { TableId = id });
        }
        [HttpPost]
        public ActionResult Add_DataFieldType(DataFieldViewModels DataField)
        {
            if (!ModelState.IsValid)
            {
                return View(DataField);
            }
            try
            {
                db.DataFields.Add(DataField);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("ProjectTable", new { id = DataField.TableId });
        }
        public ActionResult Edit_DataFieldType(int id)
        {
            var Project = db.DataFields.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult Edit_DataFieldType(DataFieldViewModels DataField)
        {
            if (!ModelState.IsValid)
            {
                return View(DataField);
            }
            try
            {
                this.db.Entry<DataFieldViewModels>(DataField).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("ProjectTable", new { id = DataField.TableId });
        }

        public ActionResult Delete_DataFieldType(int id)
        {
            var Project = db.DataFields.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [ActionName("Delete_DataFieldType")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_DataFieldTypeConfirm(int id)
        {
            var DeletedTables = db.DataFields.FirstOrDefault(emp => emp.Id == id);
            this.db.DataFields.Attach(DeletedTables);
            this.db.DataFields.Remove(DeletedTables);
            this.db.SaveChanges();
            return RedirectToAction("ProjectTable", new { id = DeletedTables.TableId });
        }




        public ActionResult DataFieldDetails(int id)
        {
            var reponse = db.DataFields.Include("DataAnnotationViewModels").Where(x => x.Id == id).FirstOrDefault();
            return View(reponse);
        }

        public ActionResult Add_DataAnnotation(int id)
        {
            return View(new DataAnnotationViewModels() { DataFieldId = id });
        }
        [HttpPost]
        public ActionResult Add_DataAnnotation(DataAnnotationViewModels DataAnnotation)
        {
            if (!ModelState.IsValid)
            {
                return View(DataAnnotation);
            }
            try
            {
                db.DataAnnotations.Add(DataAnnotation);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("DataFieldDetails", new { id = DataAnnotation.DataFieldId });
        }

        public ActionResult Edit_DataAnnotation(int id)
        {
            var Project = db.DataAnnotations.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult Edit_DataAnnotation(DataAnnotationViewModels DataAnnotation)
        {
            if (!ModelState.IsValid)
            {
                return View(DataAnnotation);
            }
            try
            {
                this.db.Entry<DataAnnotationViewModels>(DataAnnotation).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("DataFieldDetails", new { id = DataAnnotation.DataFieldId });
        }

        public ActionResult Add_DataForeignKey(int id)
        {
            ViewBag.DataFieldForeignKey = new SelectList(db.Tables, "Id", "Name");
            return View(new DataFieldForeignKeyViewModels() { TableViewID = id });
        }
        [HttpPost]
        public ActionResult Add_DataForeignKey(DataFieldForeignKeyViewModels DataFieldForeignKey)
        {
            if (!ModelState.IsValid)
            {
                return View(DataFieldForeignKey);
            }
            try
            {
                db.DataFieldForeignKeys.Add(DataFieldForeignKey);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("ProjectTable", new { id = DataFieldForeignKey.TableViewID });
        }

        public ActionResult Edit_DataForeignKey(int id)
        {
            ViewBag.DataFieldForeignKey = new SelectList(db.Tables, "Id", "Name");
            var Project = db.DataFieldForeignKeys.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult Edit_DataForeignKey(DataFieldForeignKeyViewModels TableView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DataFieldForeignKey = new SelectList(db.Tables, "Id", "Name");
                return View(TableView);
            }
            try
            {
                this.db.Entry<DataFieldForeignKeyViewModels>(TableView).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("Reponses_Mobile", new { id = TableView.TableViewID });
        }

        public ActionResult Delete_DataForeignKey(int id)
        {
            var Project = db.DataFieldForeignKeys.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [ActionName("Delete_DataForeignKey")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_DataForeignKeyConfirm(int id)
        {
            var DeletedTables = db.DataFieldForeignKeys.FirstOrDefault(emp => emp.Id == id);
            this.db.DataFieldForeignKeys.Attach(DeletedTables);
            this.db.DataFieldForeignKeys.Remove(DeletedTables);
            this.db.SaveChanges();
            return RedirectToAction("Reponses_Mobile", new { id = DeletedTables.TableViewID });
        }

        public ActionResult Add_DataForeignKey_Mobile(int id)
        {
            ViewBag.DataFieldForeignKey = new SelectList(db.Tables.Where(x=>x.DataFieldType=="Mobile"), "Id", "Name");
            return View(new DataFieldForeignKeyViewModels() { TableViewID = id });
        }
        [HttpPost]
        public ActionResult Add_DataForeignKey_Mobile(DataFieldForeignKeyViewModels DataFieldForeignKey)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DataFieldForeignKey = new SelectList(db.Tables.Where(x => x.DataFieldType == "Mobile"), "Id", "Name");
                return View(DataFieldForeignKey);
            }
            try
            {
                db.DataFieldForeignKeys.Add(DataFieldForeignKey);
                int res= db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("ProjectTable", new { id = DataFieldForeignKey.TableViewID });
        }

        public ActionResult Edit_DataForeignKey_Mobile(int id)
        {
            ViewBag.DataFieldForeignKey = new SelectList(db.Tables.Where(x => x.DataFieldType == "Mobile"), "Id", "Name");
            var Project = db.DataFieldForeignKeys.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult Edit_DataForeignKey_Mobile(DataFieldForeignKeyViewModels TableView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DataFieldForeignKey = new SelectList(db.Tables.Where(x => x.DataFieldType == "Mobile"), "Id", "Name");
                return View(TableView);
            }
            try
            {
                this.db.Entry<DataFieldForeignKeyViewModels>(TableView).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("ProjectTable", new { id = TableView.TableViewID });
        }

        public ActionResult Delete_DataForeignKey_Mobile(int id)
        {
            var Project = db.DataFieldForeignKeys.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [ActionName("Delete_DataForeignKey_Mobile")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_DataForeignKey_MobileConfirm(int id)
        {
            var DeletedTables = db.DataFieldForeignKeys.FirstOrDefault(emp => emp.Id == id);
            this.db.DataFieldForeignKeys.Attach(DeletedTables);
            this.db.DataFieldForeignKeys.Remove(DeletedTables);
            this.db.SaveChanges();
            return RedirectToAction("ProjectTable", new { id = DeletedTables.TableViewID });
        }

        public ActionResult Reponses_Mobile(int id)
        {
            ViewBag.ProjectID = id;

            var list = db.Tables.Where(x => x.ProjectID == id && x.DataFieldType == "Mobile").ToList();
            return View(list);
        }

        public ActionResult Add_Models_Mobile(int id)
        {
            return View(new TableViewModels() { ProjectID = id, DataFieldType = "Mobile" });
        }
        [HttpPost]
        public ActionResult Add_Models_Mobile(TableViewModels TableView)
        {
            if (!ModelState.IsValid)
            {
                return View(TableView);
            }
            try
            {
                db.Tables.Add(TableView);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("Reponses_Mobile", new { id = TableView.ProjectID });
        }

        public ActionResult Edit_Models_Mobile(int id)
        {
            var Project = db.Tables.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [HttpPost]
        public ActionResult Edit_Models_Mobile(TableViewModels TableView)
        {
            if (!ModelState.IsValid)
            {
                return View(TableView);
            }
            try
            {
                this.db.Entry<TableViewModels>(TableView).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                var excep = ex.Message;
            }

            return RedirectToAction("Reponses_Mobile", new { id = TableView.ProjectID });
        }

        public ActionResult Delete_Models_Mobile(int id)
        {
            var Project = db.Tables.FirstOrDefault(emp => emp.Id == id);
            return View(Project);
        }
        [ActionName("Delete_Models_Mobile")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Models_MobileConfirm(int id)
        {
            var DeletedTables = db.Tables.FirstOrDefault(emp => emp.Id == id);
            this.db.Tables.Attach(DeletedTables);
            this.db.Tables.Remove(DeletedTables);
            this.db.SaveChanges();
            return RedirectToAction("Reponses_Mobile", new { id = DeletedTables.ProjectID });
        }




    }
}