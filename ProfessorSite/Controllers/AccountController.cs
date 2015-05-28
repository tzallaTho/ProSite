using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataAccess;
using ModelEntityFrm;

namespace ProfessorSite.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(InformationClass model, string returnUrl)
        {
            //if (ModelState.IsValid)
           // {
            using (var cont = new ProfessorContext())
            {
                var user = from p in cont.users
                           where p.UserName == model.UserName && p.Password == model.Password
                           select p;

                if (user.Count() == 1) //valid credentials
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }

            }
             

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
            return View();
        }



        public ActionResult RegisterProfessor()
        {
            //fill drop down with state data
            var dd = new ProfessorContext();
            IEnumerable<State> categories = dd.states.ToList();
            ViewBag.StateList = new SelectList(categories,"id","description");
           
            return View();
        }
        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult RegisterProfessor(ProfessorClass model, string StateList)
        {
           if (ModelState.IsValid)
            {
                     var infoc = new InformationClass
                {
                    UserName = model.info.UserName,
                    Email = model.info.Email,
                    name = model.info.name,
                    surname = model.info.surname,
                    Password = model.info.Password

                };

                     State selectedsate = null;
                     if (StateList != null)
                     {
                         var dd = new ProfessorContext();
                         int selId = int.Parse(StateList);
                         selectedsate = dd.states.Single(f => f.id == selId);
                     }
                   
             
                var prof = new ProfessorClass
                {
                    info = infoc,
                    AFM = model.AFM,
                    phoneNumber = model.phoneNumber,
                    _state = selectedsate
                };
                using (var context = new ProfessorContext())
                {
                    try
                    {
                        //if username already exists

                        var user = from person in context.professors
                                   where person.info.UserName == model.info.UserName
                                   select person;
                        if (user.Count() > 1)
                        {
                            ViewBag.resultMessage = "Υπάρχει ήδη αυτό το username";
                            model = new ProfessorClass();
                            return View(model);
                        }

                        context.professors.Add(prof);
                        context.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        ViewBag.resultMessage = "Λάθος στην αποθήκευση";
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }

                       
                        //throw;
                    }
                    ViewBag.resultMessage = "Σωστή καταχώρηση δεδομένων";
                    return RedirectToAction("LogOn");
                }
            }
            else
            {
               // ModelState.AddModelError("keyName", "Form is not valid");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
