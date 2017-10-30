using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM;
using CRM.Controllers;
using CRM.Models;
using System.Data.Entity;


namespace TestCases
{
    [TestFixture]
    class Test_Seguimiento_Ventas_Controller
    {   [Category("Ventas")]
        [TestCase(20000,10,2000)]
        public void getDescuento_intValues_ReturnCorrectDiscoint(int totalVenta, int descuento, int resultadoEsperado) {
            SeguimientoVentasController segVentas = new SeguimientoVentasController();
            Assert.AreEqual(segVentas.getDescuento(totalVenta, descuento),resultadoEsperado);

        }
        [Category("Ventas")]
        [TestCase(20000, 10, 2000)]
        public void getComision_intValues_ReturnCorrectDiscoint(int totalVenta, int comision, int resultadoEsperado)
        {
            SeguimientoVentasController segVentas = new SeguimientoVentasController();
            Assert.AreEqual(segVentas.getComision(totalVenta, comision), resultadoEsperado);

        }
        [Category("Ventas")]
        [TestCase(20000, 10,5, 17000)]
        public void getTotalVenta_intValues_ReturnCorrectDiscoint(int totalVenta, int comision,int descuento, int resultadoEsperado)
        {
            SeguimientoVentasController segVentas = new SeguimientoVentasController();

            Assert.AreEqual(segVentas.getTotalVenta(totalVenta,descuento, comision), resultadoEsperado);

        }

        [TestCase(1,1,5,5,"2017/10/26","1-3;2-4;1-2;3-5", null)]
        [TestCase(1, 1, 5,null, "2017/10/26", "1-3;2-4;1-2;3-5", null)]
        [TestCase(1, 1, 5, 120, "2017/10/26", "1-3;2-4;1-2;3-5", "El porcentaje de descuento debe ser un valor entre 0 y 100")]
        [TestCase(1, 1,120, 5, "2017/10/26", "1-3;2-4;1-2;3-5", "El porcentaje de comision debe ser un valor entre 0 y 100")]
        public void registerVenta_VentaValidation_returnErrorMessage(int pCliente_id, int pVendedorId, int pComision, int pDescuento, DateTime pFecha, string pProductoLista, string expected)
        {
            Venta pVenta = new Venta();
            pVenta.cliente_id = pCliente_id;
            pVenta.comision = pComision;
            pVenta.descuento = pDescuento;
            pVenta.fecha = pFecha;
            pVenta.fvendedor_id = pVendedorId;
            pVenta.productolista = pProductoLista;

            CRM.Controllers.IVentaDB fakeDB = Substitute.For<CRM.Controllers.IVentaDB>();

            fakeDB.agregarVentaToDatabase(pVenta).Returns(30);
            fakeDB.agregarVentaXProductoToDatabase(30, 31, 32).ReturnsForAnyArgs(true);
            AgregarVenta venta = new AgregarVenta(fakeDB);
            venta.InsertarVenta(pVenta);
            Assert.AreEqual(pVenta.errorMsj, expected);
        }
        [TestCase("2014/02/31",false)]
        [TestCase("2013/03/31", true)]
        [TestCase("2013/31", false)]
        [TestCase("2013/32/32", false)]
        [TestCase("2013", false)]
        [TestCase("2016/02/29", true)]
        [TestCase("29/03/97", false)]
        [TestCase("29/03/", false)]
        public void dateIsValid_ValidDate_ReturnBool(String date, Boolean expected)
        {   
            Assert.AreEqual(AgregarVenta.fechaEsCorrecta(date),expected);
        }

        [TestCase(1, 1, 5, 5, "2017/10/26", "1-3;2-4;1-2;3-5")]
        public void registerVenta_ValidVenta_returnNull(int pCliente_id, int pVendedorId, int pComision, int pDescuento, DateTime pFecha, string pProductoLista)
        {
            Venta pVenta = new Venta();
            pVenta.cliente_id = pCliente_id;
            pVenta.comision = pComision;
            pVenta.descuento = pDescuento;
            pVenta.fecha = pFecha;
            pVenta.fvendedor_id = pVendedorId;
            pVenta.productolista = pProductoLista;

            CRM.Controllers.IVentaDB fakeDB = Substitute.For<CRM.Controllers.IVentaDB>();

            fakeDB.agregarVentaToDatabase(pVenta).Returns(30);
            fakeDB.agregarVentaXProductoToDatabase(30, 31, 32).ReturnsForAnyArgs(true);
            AgregarVenta venta = new AgregarVenta(fakeDB);
            venta.InsertarVenta(pVenta);
            Assert.AreEqual(pVenta.errorMsj, null);
        }



    }
}
