using CRM.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCases
{
    [TestFixture]
    class Test_Seguimiento_Ventas_Controller
    {
        [TestCase(20000,10,2000)]
        public void getDescuento_intValues_ReturnCorrectDiscoint(int totalVenta, int descuento, int resultadoEsperado) {
            SeguimientoVentasController segVentas = new SeguimientoVentasController();
            Assert.AreEqual(segVentas.getDescuento(totalVenta, descuento),resultadoEsperado);

        }

        [TestCase(20000, 10, 2000)]
        public void getComision_intValues_ReturnCorrectDiscoint(int totalVenta, int comision, int resultadoEsperado)
        {
            SeguimientoVentasController segVentas = new SeguimientoVentasController();
            Assert.AreEqual(segVentas.getComision(totalVenta, comision), resultadoEsperado);

        }

        [TestCase(20000, 10,5, 17000)]
        public void getTotalVenta_intValues_ReturnCorrectDiscoint(int totalVenta, int comision,int descuento, int resultadoEsperado)
        {
            SeguimientoVentasController segVentas = new SeguimientoVentasController();

            Assert.AreEqual(segVentas.getTotalVenta(totalVenta,descuento, comision), resultadoEsperado);

        }



    }
}
