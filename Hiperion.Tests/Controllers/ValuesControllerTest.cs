﻿namespace Hiperion.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Hiperion.Controllers;
    using Models;
    using Services;
    using System.Net;
    using Helpers;

    [TestFixture]
    public class ValuesControllerTest
    {
        private List<UserDto> _userList;
         
        [SetUp]
        public void InitData()
        {
             _userList = new List<UserDto>
                {
                    new UserDto
                        {
                            Id= 1,
                            Address = "Address1",
                            Name = "Name1"
                        },
                    new UserDto
                        {
                            Id= 2,
                            Address = "Address2",
                            Name = "Name2"
                        }
                };
        }

        [Test]
        public void Get()
        {
            //Arrange
            var userServiceMock = new Mock<IUserServices>();
            userServiceMock.Setup(foo => foo.GetAllUsers()).Returns(_userList);

            var controller = new UserController(userServiceMock.Object);
			controller.SetupController<UserDto>();

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
			Assert.AreEqual(2, result.GetContent<IEnumerable<UserDto>>().Count());
			Assert.AreEqual("Name1", result.GetContent<IEnumerable<UserDto>>().ElementAt(0).Name);
			Assert.AreEqual("Name2", result.GetContent<IEnumerable<UserDto>>().ElementAt(1).Name);
        }

        [Test]
        public void Post()
        {
            //Arrange
            var userServiceMock = new Mock<IUserServices>();
            userServiceMock.Setup(foo => foo.SaveOrUpdateUser(_userList.ElementAt(0))).Returns(true);
            
            var controller = new UserController(userServiceMock.Object);
            controller.SetupController<UserDto>();
          
            // Act
            var responseMessage = controller.Post(_userList.ElementAt(0));
             
            // Assert
            Assert.IsNotNull(responseMessage);
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseMessage.GetContent<UserDto>().Name, _userList.ElementAt(0).Name);

        }
    }
}
