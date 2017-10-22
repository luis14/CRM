using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM;
using CRM.Controllers;

namespace TestCases
{
    [TestFixture]
    class TestLogin
    {
        public void isValidUser()
        {
            var Login = Substitute.For<LoginController>();

        }
        
    }
}
