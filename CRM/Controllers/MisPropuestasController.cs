using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class MisPropuestasController : Controller
    {
        // GET: MisPropuestas
        public ActionResult Index()
        {
            ViewBag.propuestas = getListaPropuestas();
            return View();
        }
        public List<getPropuestas2_Result> getListaPropuestas()
        {
            List<getPropuestas2_Result> listaPropuestas = new List<getPropuestas2_Result>();
            var propuestas = new CRMEntities3();
            var getPropuestas = propuestas.getPropuestas2().ToList();
            for (int i = 0; i < getPropuestas.Count; i++)
            {
                var propuesta = getPropuestas.ElementAt(i);
                propuesta.totalVenta = getTotalVenta(propuesta.totalVenta, propuesta.descuento, propuesta.comision);
                listaPropuestas.Add(propuesta);
            }
            return listaPropuestas;
        }
        [HttpPost]
        public ActionResult updatePropuesta(Venta pVenta)
        {
            if (propuestaUpdated(pVenta))
            {
               return RedirectToAction("Index", "SeguimientoVentas");
            }
            else
            {

                ViewBag.propuestas = getListaPropuestas();
                return View("Index", pVenta);
            }
        }

        public Boolean propuestaUpdated(Venta pVenta)
        {
            try {
                var db = new CRMEntities3();
                var original = db.Ventas.Find(pVenta.venta_id);
                if (original != null)
                {
                    original.tipo = "Venta";
                    original.comision = pVenta.comision;
                    original.descuento = pVenta.descuento;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    pVenta.errorMsj = "No se puedo conectar a la base de datos";
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public double getDescuento(int? totalVenta, int? descuento)
        {
            double totalDescuento = (double)totalVenta * (double)descuento * 0.01;

            return totalDescuento;

        }

        public double getComision(int? totalVenta, int? comision)
        {
            double totalComision = (double)totalVenta * (double)comision * 0.01;
            return totalComision;
        }
        public int? getTotalVenta(int? montoTotal, int? descuento, int? comision)
        {
            double descuentoAplicado = getDescuento(montoTotal, descuento);
            double comisionAplicada = getComision(montoTotal, comision);
            int? totalVenta = montoTotal - (int)descuentoAplicado - (int)comisionAplicada;
            return totalVenta;
        }

    }

}
