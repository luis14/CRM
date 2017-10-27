﻿using NUnit.Framework;
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
    class TestLogin
    {
        ICRMEntities dbSubstitute;
        [SetUp]
        public void Setup()
        {
            dbSubstitute = Substitute.For<ICRMEntities>();
        }

        [Category("Login")]
        [TestCase("arielmmonestel", "ariel123", true)]
        [TestCase("usuarioInexistente", "ariel123", false)]
        [TestCase("arielmmonestel", "ariel12", false)]
        public void userIsValid_validUser_ReturnTrue(string pusername, string ppass, bool expected) {
            LoginController logController = new LoginController();
            User user = new User();
            user.username = pusername;
            user.pass = ppass;
            Assert.AreEqual(logController.userIsValid(user), expected);

        }

        [Category("Registro")]
        [TestCase("luisralfaro","luis123","luis123","luis1234@mail.com",null)]
        public void registerUser_validUser_ReturnTrue(String username, String pass, String repeatPass, String email,String expected) {
            IAddUserToDabase fakeDatabase = Substitute.For<IAddUserToDabase>();
            User usuario = new User { username = username, pass = pass, repeatPass = repeatPass, email = email};
            fakeDatabase.insertUserIntoDatabase(usuario).Returns(true);
            Register register = new Register(fakeDatabase);
            Assert.AreEqual(usuario.errorMessage,expected);
        }

        [Category("Registro")]
        [TestCase("PSJ2001azb",true)]
        [TestCase("Rapzod", false)]
        [TestCase("", false)]
        public void passwordIsValid_ValidPassword_ReturnBool(String password, Boolean expected)
        {
            
            Boolean esValido = Register.passwordIsValid(password);
            Assert.AreEqual(esValido,expected);
        }

        [Category("Registro")]
        [TestCase("arielmmonestel@gmail.com", true)]
        [TestCase("franciscoferfoja.es", false)]
        [TestCase("", false)]
        [TestCase("a@b.com", true)]
        [TestCase("@gmail.com", false)]
        [TestCase("juanito@hotmail", false)]

        public void emailIsValid_ValidEmail_ReturnBool(String email, Boolean expected)
        {

            Boolean esValido = Register.isValidEmail(email);
            Assert.AreEqual(esValido, expected);
        }


        [Category("Registro")]
        // correo repetido *(Revisa Uppercase tambien)
        [TestCase("luisralfaro", "Luisito123", "Luisito123", "luis1234@mail.com", "El email ya existe")]
        // usuario repetido
        [TestCase("arielmmonestel", "Luisito123", "Luisito123", "cacacacaca@mail.com", "El username ya existe")]
        // contrasenas diferentes
        [TestCase("luisralfaro1", "Contrasena1", "Contrasena2", "luis124534@mail.com", "Las contrasenas no concuerdan")]
        // sin mayuscula
        [TestCase("luisralfaro2", "luisito123", "luisito123", "luis1dd234@mail.com", "La contrasena debe tener al menos 7 caracteres y  1 Uppercase")]
        //Largo de contrasena
        [TestCase("luisralfaro4", "lui123", "lui123", "luis12ff34@mail.com", "La contrasena debe tener al menos 7 caracteres y  1 Uppercase")] 
        public void registerUser_UserValidation_returnErrorMessage(String username, String pass, String repeatPass, String email, string msjExpected)
        {
           // var fakeRegister = Substitute.For<Register>();
            LoginController LogController = new LoginController();
            User usuario = new User { username = username, pass = pass, repeatPass = repeatPass, email = email };
            //fakeRegister.RegisterUser(usuario).Returns(usuario);
            LogController.register(usuario);
            Assert.AreEqual(usuario.errorMessage, msjExpected);


        }
        

        /*

                [Ignore("no correr")]
                [TestCase("arielmmonestel","ariel123",true)]
                [TestCase("arielmmonestel", "ariel12", false)]
                public void userIsValid_validUser_RetunsTrue(string pusername, string ppass,bool expected)
                {


                    dbSubstitute = Substitute.For<ICRMEntities>();
                    LoginController loginFake = new LoginController(dbSubstitute);
                    User user = new User();
                    user.username = pusername;
                    user.pass = ppass;
                    List<User> listaRetorno = new List<User>();
                    List<User> userlist = new List<User>();
                    userlist.Add(user);
                    SetupUsersData(userlist);
                    // Capture the fact that a new session is created and added.

                    dbSubstitute.Users.When(usuario =>
                    {
                        usuario.Where<User>(x => x.username == user.username && x.pass == user.pass).FirstOrDefault();
                    }).Do((callInfo) =>
                    {   

                        var usuarioRetornado =  callInfo.Arg<User>();
                        listaRetorno.Add(usuarioRetornado);
                        //var session = callInfo.Arg > Session > ();
                        //newlyAddedSession = session.Id;
                        //sessions.Add(session);
                    });
                    Assert.AreEqual(listaRetorno.Count, 1);


                }

                private void SetupUsersData(List<User> users)
                {
                    var queryable = users.AsQueryable();
                    var mock = Substitute.For<IDbSet<User>, DbSet<User>>().Initialize(queryable);
                    dbSubstitute.Users.Returns(mock);
                }


            */
    }
}
