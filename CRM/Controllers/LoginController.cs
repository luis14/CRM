using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Autherize(CRM.Models.User userModel)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var userDetails = db.Users.Where( x => x.username == userModel.username && x.pass == userModel.pass).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.errorMessage = "Usuario o contrasena incorrectos";
                    return View("Index", userModel);
                }
                else {
                    Session["userID"] = userDetails.ID;
                    Session["username"] = userDetails.username;
                    ViewBag.username = Session["username"];
                    return RedirectToAction("Index", "Home");
                }
            }
                
        }
        [HttpPost]
        public ActionResult Register(CRM.Models.User userModel)
        {
            using (CRMEntities db = new CRMEntities())
            {


                if (!userModel.pass.Equals(userModel.repeatPass))
                {
                    userModel.errorMessage = "Las contrasenas no coinciden";
                    return View("Index", userModel);
                }
                var userDetails = db.Users.Where(x => x.username.ToUpper() == userModel.username.ToUpper()).FirstOrDefault(); 
                if (userDetails != null) {
                    userModel.errorMessage = "El nombre de usuario ya han sido usado";
                    return View("Index", userModel);
                }
                 userDetails = db.Users.Where(x => x.email.ToUpper() == userModel.email.ToUpper()).FirstOrDefault();
                if (userDetails != null)
                {
                    userModel.errorMessage = "El correo electronico ya han sido usados";
                    return View("Index", userModel);
                }
                else {
                    var userInsert = db.Users.Add(userModel);
                    db.SaveChanges();
                    Session["userID"] = userModel.ID;
                    Session["username"] = userModel.username;
                    ViewBag.username = Session["username"];
                    return RedirectToAction("Index", "Home");
                }
            }

        }

    }
}