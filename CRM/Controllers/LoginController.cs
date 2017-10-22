using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public interface ILogin
    {
         Boolean userIsValid(CRM.Models.User usuarioNoIdentificado);
         ActionResult Autherize(CRM.Models.User userModel);
         ActionResult Register(CRM.Models.User userModel);


    }
    public class LoginController : Controller, ILogin
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public Boolean userIsValid(CRM.Models.User usuarioNoIdentificado) {
            bool esValido = false;

            using (CRMEntities db = new CRMEntities())
            {
                var userDetails = db.Users.Where(x => x.username == usuarioNoIdentificado.username && x.pass == usuarioNoIdentificado.pass).FirstOrDefault();
                if (userDetails != null)
                {
                    esValido = true;
                }
                
            }
                return esValido;
        }
        [HttpPost]
        public ActionResult Autherize(CRM.Models.User userModel)
        {
            if (userIsValid(userModel)) {
                Session["username"] = userModel.username;
                ViewBag.username = Session["username"];
                return RedirectToAction("Index", "Home");
            }
            else
            {
                userModel.errorMessage = "Usuario o contrasena incorrectos";
                return View("Index", userModel);
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