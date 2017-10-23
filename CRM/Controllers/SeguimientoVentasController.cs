using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CRM.Controllers
{
    public class SeguimientoVentasController : Controller
    {
        public double getDescuento(int? totalVenta, int? descuento)
        {
            double totalDescuento = (double)totalVenta * (double)descuento * 0.01;

            return totalDescuento;

        }

        public double getComision(int? totalVenta, int? comision) {
            double totalComision = (double)totalVenta * (double)comision * 0.01;
            return totalComision;
        }
        public int? getTotalVenta(int? montoTotal, int? descuento, int? comision) {
            double descuentoAplicado = getDescuento(montoTotal, descuento);
            double comisionAplicada = getComision(montoTotal, comision);
            int? totalVenta = montoTotal - (int)descuentoAplicado - (int)comisionAplicada;
            return totalVenta;
        }

        // GET: SeguimientoVentas
        public ActionResult Index()
        {
            var ventas = new CRMEntities3();
            var getVentas = ventas.getVentas().ToList();
            for (int i = 0; i < getVentas.Count; i++) {
                var venta = getVentas.ElementAt(i);
                venta.totalVenta = getTotalVenta(venta.totalVenta, venta.descuento, venta.comision);
            }
            ViewBag.listaVentas = getVentas;
            return View();
        }

        public ActionResult FormNewSale() {
            var empleados = new CRMEntities3();
            var getEmpleados = empleados.Vendedores.ToList();
            var items = new List<SelectListItem>();
            for (var i = 0; i < getEmpleados.Count(); i++) {

                items.Add(new SelectListItem { Text = getEmpleados.ElementAt(i).nombre.ToString(), Value = getEmpleados.ElementAt(i).vendedor_id.ToString()});
            }
            ViewBag.listaEmpleados = items;

            var getClientes = empleados.Clientes.ToList();
            items = new List<SelectListItem>();
            for (var i = 0; i < getClientes.Count(); i++)
            {

                items.Add(new SelectListItem { Text = getClientes.ElementAt(i).nombre.ToString(), Value = getClientes.ElementAt(i).cliente_id.ToString() });
            }
            ViewBag.listaClientes = items;


            var getProductos= empleados.Productos.ToList();
            items = new List<SelectListItem>();
            for (var i = 0; i < getProductos.Count(); i++)
            {

                items.Add(new SelectListItem { Text = getProductos.ElementAt(i).nombre.ToString(), Value = getProductos.ElementAt(i).producto_id.ToString() });
            }
            ViewBag.listaProductos = items;
            return View();
        }

        [HttpPost]
        public ActionResult addVenta(CRM.Models.Venta venta)
        {
            var producCantidad = venta.productolista;
            venta.Ventas_x_Productos = null;
            var ventaId = -1;
            using (CRMEntities3 db = new CRMEntities3())
            {
                
               
                var insert = db.Ventas.Add(venta);
                db.SaveChanges();
                db.Entry(venta).GetDatabaseValues();
                 ventaId = venta.venta_id;

            }
            var listaProductosyCantidad = producCantidad.Split(';');
            for (var i = 0; i < listaProductosyCantidad.Length; i++) {
                var elementos = listaProductosyCantidad.ElementAt(i).Split('-');
                var producto = elementos.ElementAt(0);
                var cantidad = elementos.ElementAt(1);
                Ventas_x_Productos ventaProducto = new Ventas_x_Productos();
                ventaProducto.f_venta_id = int.Parse(ventaId.ToString());
                ventaProducto.f_producto_id = int.Parse(producto.ToString());
                ventaProducto.cantidad = int.Parse(cantidad.ToString());

                using (CRMEntities3 db = new CRMEntities3())
                {

                    var insert = db.Ventas_x_Productos.Add(ventaProducto);
                    db.SaveChanges();

                }
            }
            return RedirectToAction("Index", "SeguimientoVentas");
        }

        [HttpPost]
        public void getProductos()
        {
            var productos = new CRMEntities3();
            var getProductos = productos.Productos.ToList();
            List<Producto> listaProductos = new List<Producto>(); 
            for (var i = 0; i < getProductos.Count(); i++){
                var productoActual= getProductos.ElementAt(i);
                Producto producto = new Producto();
                producto.producto_id = productoActual.producto_id;
                producto.nombre = productoActual.nombre;
                producto.precio = productoActual.precio;
                listaProductos.Add(producto);
 
            }
            string output = JsonConvert.SerializeObject(listaProductos);
            Response.Write(output);
            Response.Flush();
            Response.End();
        }
    }
}