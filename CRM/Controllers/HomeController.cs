using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var clientes = new CRMEntities1();
            ViewBag.clientList = clientes.Clientes.ToList();
           // ViewBag.username = Session["username"];
            //Console.Write("Nombre de usuario> " + ViewBag.username);
            return View();
        }
    }
}