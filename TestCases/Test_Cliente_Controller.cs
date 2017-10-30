
﻿
﻿using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Controllers;
using CRM.Models;
using System.Data.Entity;

namespace TestCases
{
    [TestFixture]
    class Test_Cliente_Controller
    {
        [TestCase("alonso@mail.com", "Alonso", "Costa Rica", null, "Persona", "Debe llenar todos los campos solicitados.")]
        [TestCase("info@cerveceria.cr", "Cerveceria de Costa Rica", "Costa Rica", "+50680803020", "Empresa", "El cliente ya fue registado previamente")]
        [TestCase("alonso.mail.com", "Alonso", "Costa Rica", "88888888", "Persona", "El correo no es válido")]
        public void addCliente_ClienteValidation_ReturnErrorMessage(string correo, string nombre, string pais, string telefono, string tipo, string Expected)
        {
            ClienteController stub_ClienteController = new ClienteController();
            Cliente cliente = new Cliente { correo = correo, nombre = nombre, pais = pais, telefono = telefono, tipoCliente = tipo };
            //stub_ClienteController.InsertarClienteEnBD(cliente).Returns(true);
            stub_ClienteController.agregarCliente(cliente);
            Assert.AreEqual(cliente.errorMsj, Expected);
        }

        [TestCase("info@marvelmagazine.com", "Marvel CO", "Costa Rica", "00157484638474", "Empresa", null)]
        [TestCase("mariaRamos@mail.com", "Maria", "Costa Rica", "50694736284", "Persona", null)]
        [TestCase("alonso@mail.com", "Alonso", "Costa Rica", "88888888", "Persona", null)]
        public void registerCliente_ClienteValidation_ReturnNull(string correo, string nombre, string pais, string telefono, string tipo, string Expected)
        {
            var stub_AddCliente = Substitute.For<IClienteBD>();
            ClienteController controller = new ClienteController(stub_AddCliente);
            Cliente cliente = new Cliente { correo = correo, nombre = nombre, pais = pais, telefono = telefono, tipoCliente = tipo };
            stub_AddCliente.InsertarClienteEnBD(cliente).Returns(true);
            controller.agregarCliente(cliente);
            Assert.AreEqual(cliente.errorMsj, Expected);
        }
    }
}
