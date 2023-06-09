﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeCrud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models;
using System.Net.Http.Headers;
using System.Net;
using Moq;
using Xunit;
using System.Numerics;
using Newtonsoft.Json;

namespace EmployeeCrud.Services.Tests
{
    [TestClass]
    public class EmployeeDetailsServicesTests
    {
        private Mock<IEmployeeDetailsServices>? _employeeDetailsServices; 

        [TestInitialize]
        public void Initialize()
        {
            _employeeDetailsServices = new Mock<IEmployeeDetailsServices>();
        }

        [TestMethod]
        public async Task GetCallTest()
        {
            // Arrange
            EmployeeDetailsServices employeeDetails = new EmployeeDetailsServices();
            // Act
            var response = await employeeDetails.GetCall("users");
            // Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task PostCallTest()
        {
            // Arrange
            EmployeeDetailsServices employeeDetails = new EmployeeDetailsServices();
            var mockEmail = GenerateRandomEmail();
            var Employee = new Employee { Id = 0, Name = "Testdata", Email = mockEmail, Gender = "male", Status = "active" };
            // Act
            var response = await employeeDetails.PostandPutCall(Employee);
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task PutCallTest()
        {
            // Arrange
            EmployeeDetailsServices employeeDetails = new EmployeeDetailsServices();
            var response = await employeeDetails.GetCall("users");
            var employee = JsonConvert.DeserializeObject<List<Employee>>(response);
            // Act
            var responseStatus = await employeeDetails.PostandPutCall(employee[1]);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, responseStatus.StatusCode);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task PostandPutCallNullTest()
        {

            // Arrange
            EmployeeDetailsServices employeeDetails = new EmployeeDetailsServices();
            var Employee = new Mock<Employee>();
            // Act
            var response = await employeeDetails.PostandPutCall(Employee.Object);
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteCallTest()
        {
            // Arrange
            EmployeeDetailsServices employeeDetails = new EmployeeDetailsServices();
            var response = await employeeDetails.GetCall("users");
            var employee = JsonConvert.DeserializeObject<List<Employee>>(response);
            // Act
            var responseStatus = await employeeDetails.DeleteCall(employee[0].Id);
            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, responseStatus);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task DeleteCallNullTest()
        {
            // Arrange
            EmployeeDetailsServices employeeDetails = new EmployeeDetailsServices();
            var Employee = new Mock<Employee>();
            // Act
            var responseStatus = await employeeDetails.DeleteCall(Employee.Object.Id);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, responseStatus);
            Assert.IsNotNull(responseStatus);
        }

        public static string GenerateRandomEmail()
        {
            return string.Format("{0}@{1}.com", GenerateRandomAlphabetString(10), GenerateRandomAlphabetString(10));
        }
        public static string GenerateRandomAlphabetString(int length)
        {
            string allowedChars = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var rnd = SeedRandom();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rnd.Next(allowedChars.Length)];
            }

            return new string(chars);
        }
        private static Random SeedRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
        }
    }
}