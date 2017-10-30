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

        // GET: SeguimientoVentas
        public ActionResult Index()
        {
            var ventas = new CRMEntities3();
            var getVentas = ventas.getVentas().ToList();
            for (int i = 0; i < getVentas.Count; i++)
            {
                var venta = getVentas.ElementAt(i);
                venta.totalVenta = getTotalVenta(venta.totalVenta, venta.descuento, venta.comision);
            }
            ViewBag.listaVentas = getVentas;
            return View();
        }

        public ActionResult FormNewSale()
        {
            ViewBag.listaEmpleados = getListaEmpleados();
            ViewBag.listaClientes = getListaClientes();
            ViewBag.listaProductos = getListaProductos();
            return View();
        }

        public List<SelectListItem> getListaEmpleados()
        {
            var empleados = new CRMEntities3();
            var getEmpleados = empleados.Vendedores.ToList();
            var items = new List<SelectListItem>();
            for (var i = 0; i < getEmpleados.Count(); i++)
            {

                items.Add(new SelectListItem { Text = getEmpleados.ElementAt(i).nombre.ToString(), Value = getEmpleados.ElementAt(i).vendedor_id.ToString() });
            }
            return items;
        }

        public List<SelectListItem> getListaClientes()
        {
            var empleados = new CRMEntities3();
            var getClientes = empleados.Clientes.ToList();
            var items = new List<SelectListItem>();
            for (var i = 0; i < getClientes.Count(); i++)
            {

                items.Add(new SelectListItem { Text = getClientes.ElementAt(i).nombre.ToString(), Value = getClientes.ElementAt(i).cliente_id.ToString() });
            }
            return items;
        }

        public List<SelectListItem> getListaProductos()
        {
            var  empleados = new CRMEntities3();
            var getProductos = empleados.Productos.ToList();
            var items = new List<SelectListItem>();
            for (var i = 0; i < getProductos.Count(); i++)
            {

                items.Add(new SelectListItem { Text = getProductos.ElementAt(i).nombre.ToString(), Value = getProductos.ElementAt(i).producto_id.ToString() });
            }
            return items;
        }

        [HttpPost]
        public ActionResult addVenta(CRM.Models.Venta venta)
        {
            AgregarVenta ventaDB = new AgregarVenta();
            Boolean insertoBien =  false;
            insertoBien = ventaDB.InsertarVenta(venta);
            if (insertoBien) { 
                return RedirectToAction("Index", "SeguimientoVentas");
            }
            else
            {
            ViewBag.listaEmpleados = getListaEmpleados();
            ViewBag.listaClientes = getListaClientes();
            ViewBag.listaProductos = getListaProductos();
                return View("FormNewSale", venta);
            }
        }


        [HttpPost]
        public void getProductos()
        {
            var productos = new CRMEntities3();
            var getProductos = productos.Productos.ToList();
            List<Producto> listaProductos = new List<Producto>();
            for (var i = 0; i < getProductos.Count(); i++)
            {
                var productoActual = getProductos.ElementAt(i);
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

    public class AgregarVenta
    {
        IVentaDB ventaDB = new VentaDB();

        public AgregarVenta() { }
        public AgregarVenta(IVentaDB pVentaDB)
        {
            ventaDB = pVentaDB;
        }
        public Boolean validateVenta(Venta pVenta)
        {
            Boolean ventaEsValida = true;

            if ((pVenta.cliente_id == null) || (pVenta.comision == null) || (pVenta.descuento == null) || (pVenta.fecha == null) || (pVenta.fvendedor_id == null) || (pVenta.productolista == null))
            {
                ventaEsValida = false;
                pVenta.errorMsj = "Debe llenar todos campos";

            }
            else if (pVenta.descuento < 0 || pVenta.descuento > 100)
            {
                ventaEsValida = false;
                pVenta.errorMsj = "El porcentaje de descuento debe ser un valor entre 0 y 100";
            }
            else if (pVenta.comision < 0 || pVenta.comision > 100)
            {
                ventaEsValida = false;
                pVenta.errorMsj = "El porcentaje de comision debe ser un valor entre 0 y 100";
            }
            else if (!fechaEsCorrecta(pVenta.fecha.ToString()))
            {
                ventaEsValida = false;
                pVenta.errorMsj = "La fecha no es correcta";
            }


            return ventaEsValida;

        }

        public static bool fechaEsCorrecta(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean InsertarVenta(Venta pVenta)
        {
            Boolean insertoVenta = false;

            if (validateVenta(pVenta))
            {
                int ventaId =  ventaDB.agregarVentaToDatabase(pVenta);
                pVenta.Ventas_x_Productos = null;
                var producCantidad = pVenta.productolista;
                var listaProductosyCantidad = producCantidad.Split(';');
                for (var i = 0; i < listaProductosyCantidad.Length; i++)
                {
                    var elementos = listaProductosyCantidad.ElementAt(i).Split('-');
                    var producto = int.Parse(elementos.ElementAt(0));
                    var cantidad = int.Parse(elementos.ElementAt(1));
                    Boolean insertoProducto = InsertarVentaXProducto(ventaId, producto, cantidad);
                }

                if(ventaId > 0)
                {
                    insertoVenta = true;
                }
            }
            return insertoVenta;

        }
        public Boolean InsertarVentaXProducto(int pVentaId, int pProductoID, int pCantidad)
        {
            Boolean insertoCorrectamente = ventaDB.agregarVentaXProductoToDatabase(pVentaId, pProductoID, pCantidad);
            return insertoCorrectamente;
        }
    }
        public interface IVentaDB
        {
            int agregarVentaToDatabase(Venta pVenta);
            bool agregarVentaXProductoToDatabase(int pVentaId, int pProductoID, int pCantidad);

        }
        public class VentaDB : IVentaDB
        {


            public bool agregarVentaXProductoToDatabase(int pVentaId, int pProductoID, int pCantidad)
            {

                Ventas_x_Productos ventaProducto = new Ventas_x_Productos();
                ventaProducto.f_venta_id = int.Parse(pVentaId.ToString());
                ventaProducto.f_producto_id = int.Parse(pProductoID.ToString());
                ventaProducto.cantidad = int.Parse(pCantidad.ToString());

                using (CRMEntities3 db = new CRMEntities3())
                {

                    var insert = db.Ventas_x_Productos.Add(ventaProducto);
                    db.SaveChanges();
                    if (insert != null)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }


            public int agregarVentaToDatabase(Venta pVenta)
            {
                int ventaId = -1;
                using (CRMEntities3 db = new CRMEntities3())
                {
                    var insert = db.Ventas.Add(pVenta);
                    db.SaveChanges();
                    db.Entry(pVenta).GetDatabaseValues();
                    ventaId = pVenta.venta_id;

                }
                return ventaId;
            }
        }
    }
