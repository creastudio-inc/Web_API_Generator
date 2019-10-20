using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Web_API_Generator.Controllers
{
    public class GeneratorController : Controller
    {
        // GET: Generator
        public ActionResult Index(int? ProjectID)
        {
            ViewBag.ProjectID = ProjectID;
            return View();
        }



        /// <summary>
        /// Action for triggering long running operation
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Operation(int? ProjectID, GeneratorLibrary.Enum.GeneratorRequest TypeOperation)
        {
            HttpSessionStateBase session = Session;
            if (TypeOperation == GeneratorLibrary.Enum.GeneratorRequest.Enum)
            {
                session["OPERATION_PROGRESS_TEXT"] = "No ENum";
                return Json(new { progress = 100,text="No ENum" });
            }
            if (TypeOperation == GeneratorLibrary.Enum.GeneratorRequest.BaseModel)
            {
                //Separate thread for long running operation
                ThreadPool.QueueUserWorkItem(delegate
                {
                    int operationProgress;
                    for (operationProgress = 0; operationProgress <= 100; operationProgress = operationProgress + 2)
                    {
                        session["OPERATION_PROGRESS"] = operationProgress;
                        session["OPERATION_PROGRESS_TEXT"] = "Started BaseModel " + operationProgress;
                        Thread.Sleep(100);
                    }
                });

                return Json(new { progress = 0,text= "Started BaseModel" });
            }
            if (TypeOperation == GeneratorLibrary.Enum.GeneratorRequest.Models)
            {
                //Separate thread for long running operation
                ThreadPool.QueueUserWorkItem(delegate
                {
                    int operationProgress;
                    for (operationProgress = 0; operationProgress <= 100; operationProgress = operationProgress + 2)
                    {
                        session["OPERATION_PROGRESS"] = operationProgress;
                        session["OPERATION_PROGRESS_TEXT"] = "Started Models" + operationProgress;
                        Thread.Sleep(100);
                    }
                });

                return Json(new { progress = 0,text= "Started Models" });
            }
            if (TypeOperation == GeneratorLibrary.Enum.GeneratorRequest.MobileModels)
            {
                //Separate thread for long running operation
                ThreadPool.QueueUserWorkItem(delegate
                {
                    int operationProgress;
                    for (operationProgress = 0; operationProgress <= 100; operationProgress = operationProgress + 2)
                    {
                        session["OPERATION_PROGRESS"] = operationProgress;
                        session["OPERATION_PROGRESS_TEXT"] = "Started MobileModels "+ operationProgress;
                        Thread.Sleep(100);
                    }
                });

                return Json(new { progress = 0,text= "Started MobileModels" });
            }
            if (TypeOperation == GeneratorLibrary.Enum.GeneratorRequest.Context)
            {
                //Separate thread for long running operation
                ThreadPool.QueueUserWorkItem(delegate
                {
                    int operationProgress;
                    for (operationProgress = 0; operationProgress <= 100; operationProgress = operationProgress + 2)
                    {
                        session["OPERATION_PROGRESS"] = operationProgress;
                        session["OPERATION_PROGRESS_TEXT"] = "Started Context "+ operationProgress;
                        Thread.Sleep(100);
                    }
                });

                return Json(new { progress = 0,text= "Started Context" });
            }
            if (TypeOperation == GeneratorLibrary.Enum.GeneratorRequest.Migrations)
            {
                //Separate thread for long running operation
                ThreadPool.QueueUserWorkItem(delegate
                {
                    int operationProgress;
                    for (operationProgress = 0; operationProgress <= 100; operationProgress = operationProgress + 2)
                    {
                        session["OPERATION_PROGRESS"] = operationProgress;
                        session["OPERATION_PROGRESS_TEXT"] = "Started Migrations" + operationProgress;

                        Thread.Sleep(100);
                    }
                });

                return Json(new { progress = 0,text= "Started Migrations" });
            }

            return Json(new { progress = 100,text="error" });
        }

        public ActionResult OperationProgress()
        {
            int operationProgress = 0;

            if (Session["OPERATION_PROGRESS"] != null)
            {
                operationProgress = (int)Session["OPERATION_PROGRESS"];

            }
            else
            {
                operationProgress = 100;
            }

            return Json(new { progress = operationProgress,text= Session["OPERATION_PROGRESS_TEXT"] }, JsonRequestBehavior.AllowGet);
        }



        // WaincomModels
        public ActionResult Model()
        {
            // Attributes

            // DataClass

            // Enum

            // MobileModel

            // ViewModel

            return View();
        }
        // WaincomCore
        public ActionResult Core()
        {
            // Context

            // Exception

            // Interfaces

            // Repository

            // Synchronizer

            return View();
        }
    }
}