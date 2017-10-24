using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
                if (clienteEsValido(cliente)) { 
                    var insert = db.Clientes.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else{
                    return View("Index", cliente);
                }
            }
        }


        public Boolean clienteEsValido(Cliente pCliente)
        {
            CRMEntities1 db = new CRMEntities1();
            bool clienteEsValido = true;
            if(pCliente.correo == null || pCliente.nombre == null || pCliente.pais == null || pCliente.telefono == null || pCliente.tipoCliente == null) {
                clienteEsValido = false;
                pCliente.errorMsj = ("Debe llenar todos los campos solicitados.");
            }
            var clientDetails = db.Clientes.Where(x => x.correo == pCliente.correo || x.nombre == pCliente.nombre).FirstOrDefault();
            if (clientDetails != null)
            {
                clienteEsValido = false;
                pCliente.errorMsj = ("El cliente ya fue registado previamente");
            }
            if (!isValidEmail(pCliente.correo))
            {
                clienteEsValido = false;
                pCliente.errorMsj = "El correo no es válido";
            }
            return clienteEsValido;
        }

        public static bool isValidEmail(string emailaddress)
        {   if(emailaddress != null) { 
                    try
                    {
                        MailAddress m = new MailAddress(emailaddress);

                        return true;
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
    }
}