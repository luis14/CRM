using CRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
   
    public class LoginController : Controller
    {
        // GET: Login

        private ICRMEntities db = new CRMEntities();

        public LoginController(ICRMEntities p_db)
        {
            db = p_db;
        }
        public LoginController() {
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
            if (userModel.username == null || userModel.pass == null)
            {
                userModel.errorMessage = "Debe llenar los campos solicitados";
                return View("Index", userModel);
            }
            else {
                if (userIsValid(userModel)) {
                    string username = userModel.username;
                    Session["username"] = username;
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
           Register registerObject = new Register();
           registerObject.RegisterUser(userModel);
           return View("Index", userModel);
        }


    }

    public class Register {

        IAddUserToDabase database = new AddUserToDatabase();

        public Register(IAddUserToDabase pdatabase) {
            database = pdatabase;
        }
        public Register() { }

        public void RegisterUser(User pUser)
        {
            if (Register.validateUser(pUser))
            {
               database.insertUserIntoDatabase(pUser);
            
            }

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
        public static bool isValidEmail(string emailaddress)
        {
            if (emailaddress != null)
            {
                try
                {
                    var emailAddress =  new EmailAddressAttribute();
                    return emailAddress.IsValid(emailaddress);
                     
                }

                catch (FormatException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static Boolean validateUser(User pUser) {
            Boolean userIsValid = true;
            try {
                if (pUser.email == null || pUser.username == null || pUser.pass == null || pUser.repeatPass == null) {
                    userIsValid = false;
                    pUser.errorMessage = "Debe llenar todos campos";
                }
                else if (!pUser.pass.Equals(pUser.repeatPass)) {
                    userIsValid = false;
                    pUser.errorMessage = "Las contrasenas no concuerdan";
                }
                else if (!passwordIsValid(pUser.pass))
                {
                    userIsValid = false;
                    pUser.errorMessage = "La contrasena debe tener al menos 7 caracteres y  1 Uppercase";
                }

                else if (!Register.isValidEmail(pUser.email))
                {
                    userIsValid = false;
                    pUser.errorMessage = "El correo no es válido";
                }
                ICRMEntities db = new CRMEntities();
                var userDetails = db.Users.Where(x => x.username == pUser.username).FirstOrDefault();


                if (userDetails != null)
                {
                    userIsValid = false;
                    pUser.errorMessage = "El username ya existe";
                }

                userDetails = db.Users.Where(x => x.email.ToUpper() == pUser.email.ToUpper()).FirstOrDefault();

                if (userDetails != null)
                {
                    userIsValid = false;
                    pUser.errorMessage = "El email ya existe";
                }
               
            }
            catch (Exception)
            {

                pUser.errorMessage = "Debe llenar todos campos";
            }
            return userIsValid;
        }




    }
    public interface IAddUserToDabase{
        Boolean insertUserIntoDatabase(User user);
    }

    public class AddUserToDatabase:IAddUserToDabase
    {
        public Boolean insertUserIntoDatabase(User user) {
            Boolean insertoCorrectamente = false;
            try { 
                CRMEntities db = new CRMEntities();
                var userInserted = db.Users.Add(user);
                db.SaveChanges();
                if(userInserted != null) {
                    insertoCorrectamente = true;
                }
            }catch(Exception ex)
            {
                insertoCorrectamente = false;
            }

            return insertoCorrectamente;

        }

    }
}