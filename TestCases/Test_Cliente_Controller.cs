
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
        public void registerCliente_ClienteValidation_ReturnErrorMessage(string correo, string nombre, string pais, string telefono, string tipo, string msjExpected)
        {
            ClienteController ClienteController = new  ClienteController();
          //  ClienteController.
            //pCliente.correo == null || pCliente.nombre == null || pCliente.pais == null || pCliente.telefono == null || pCliente.tipoCliente == null
        }
    }
}
