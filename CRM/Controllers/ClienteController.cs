using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addCliente(CRM.Models.Cliente cliente)
        {
            using (CRMEntities1 db = new CRMEntities1()) {
                var insert = db.Clientes.Add(cliente);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}