
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
            var stub_ClienteController = Substitute.For<ClienteController>();
            Cliente cliente = new Cliente { correo = correo, nombre = nombre, pais = pais, telefono = telefono, tipoCliente = tipo };
            //stub_ClienteController.insertClientToDatabase(cliente).Returns(true);
            stub_ClienteController.addCliente(cliente);
            Assert.AreEqual(cliente.errorMsj, Expected);
        }

        [Category("Registro Persona")]
        [Ignore("Inserta en la base de datos")]
        [TestCase("alonso@mail.com", "Alonso", "Costa Rica", "88888888", "Persona", null)]
        public void registerCliente_ClienteValidation_ReturnNull(string correo, string nombre, string pais, string telefono, string tipo, string Expected)
        {
            var stub_ClienteController = Substitute.For<ClienteController>();
            Cliente cliente = new Cliente { correo = correo, nombre = nombre, pais = pais, telefono = telefono, tipoCliente = tipo };
            stub_ClienteController.insertClientToDatabase(cliente).Returns(true);
            stub_ClienteController.addCliente(cliente);
            Assert.AreEqual(cliente.errorMsj, Expected);
        }
    }
}
