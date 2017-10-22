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

namespace TestCases
{
    [TestFixture]
    class TestLogin
    {
        public void isValidUser()
        {
            var login = Substitute.For<LoginController>();
            var fakeUser = Substitute.For<User>();
            fakeUser.username = "luis";
            fakeUser.pass = "luis1234"
            login.Autherize(fakeUser).Returns();

        }
        
    }
}
