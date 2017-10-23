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
         ActionResult register(CRM.Models.User userModel);


    }
    public class LoginController : Controller, ILogin
    {
        // GET: Login

        private ICRMEntities db = new CRMEntities();
        private Register registerObject;
        public LoginController(ICRMEntities p_db)
        {
            db = p_db;
        }

        public LoginController(Register preg) {
            registerObject = preg;
        }
        public LoginController() {
            registerObject = new Register();
        }
        public ActionResult Index()
        {
            return View();
        }

        public Boolean userIsValid(CRM.Models.User usuarioNoIdentificado) {
 
            bool esValido = false;
            
                var userDetails = db.Users.Where(x => x.username == usuarioNoIdentificado.username && x.pass == usuarioNoIdentificado.pass).FirstOrDefault();
                if (userDetails != null)
                {
                    esValido = true;
                }
                
            
                return esValido;
        }
        [HttpPost]
        public ActionResult Autherize(CRM.Models.User userModel)
        {
            if (userModel.username.Equals("") || userModel.pass.Equals(""))
            {
                userModel.errorMessage = "Debe llenar los campos solicitados";
                return View("Index", userModel);
            }
            else { 
            if (Register.validateUser(userModel)) {
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

        }

        [HttpPost]
        public ActionResult register(CRM.Models.User userModel)
        {
            using (CRMEntities db = new CRMEntities())
            {
                if (userIsValid(userModel)) {

                   
                    registerObject.RegisterUser(userModel);
                    //  var userInsert = db.Users.Add(userModel);
                    //db.SaveChanges();
                    //Autherize(userInsert);
                    return null;

                }
              
                else {

                    return View("Index", userModel);
                }
            }

        }


    }

    public class Register {
        public ILogin login;

        public Register() {
            login = new LoginController();
        }
        public static Boolean passwordIsValid(String newPass)
        {
            if
                (
                    newPass.Length > 6 &&        //if length is >= 6
                    newPass.Any(char.IsUpper) //if any character is upper case
                )
            {

                return true;
            }
            else {
                return false;
            }


        }
        public static Boolean validateUser(User pUser) {
            Boolean userIsValid = true;

            if (pUser.email.Equals("") || pUser.username.Equals("") || pUser.pass.Equals("") || pUser.repeatPass.Equals("")) {
                userIsValid = false;
                pUser.errorMessage = "Debe llenar todos campos";
            }
            if (!pUser.pass.Equals(pUser.repeatPass)) {
                userIsValid = false;
                pUser.errorMessage = "Las contrasenas no concuerdan";
            }
             ICRMEntities db = new CRMEntities();
             var userDetails = db.Users.Where(x => x.username == pUser.username ).FirstOrDefault();


            if (userDetails != null)
            {
                userIsValid = false;
                pUser.errorMessage = "El username ya existe";
            }
            userDetails = db.Users.Where(x => x.email == pUser.email).FirstOrDefault();

            if (userDetails != null)
            {
                userIsValid = false;
                pUser.errorMessage = "El email ya existe";
            }
            if (!passwordIsValid(pUser.pass)) {
                userIsValid = false;
                pUser.errorMessage = "La contrasena debe tener al menos 7 caracteres y  1 Uppercase";
            }
            return userIsValid;
        }

        public Register(ILogin pLogin)
        {
            login = pLogin;
        }

        public User RegisterUser(User user)
        {
            CRMEntities db = new CRMEntities();
            var userInserted = db.Users.Add(user);
            db.SaveChanges();
            login.Autherize(user);
            return userInserted;
        }


    }
}