using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass()]
    public class EmployeeDetailsAPITests
    {
        [TestMethod()]
        public async Task GetCallTest()
        {
            // Arrange
            EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
            // Act
            var response = await employeeDetails.GetCall("users");
            // Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task PostCallTest()
        {
            // Arrange
            EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
            var Employee = new Employee { Id = 0, Name = "perarivalan", Email = "perarivalan@gmail.com", Gender = "male", Status = "active" };
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
            EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
            var response = await employeeDetails.GetCall("users");
            var employee = JsonConvert.DeserializeObject<List<Employee>>(response);
            // Act
            var responseStatus = await employeeDetails.PostandPutCall(employee[1]);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, responseStatus.StatusCode);
            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public async Task PostandPutCallNullTest()
        {

            // Arrange
            EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
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
            EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
            var response = await employeeDetails.GetCall("users");
            var employee = JsonConvert.DeserializeObject<List<Employee>>(response);
            // Act
            var responseStatus = await employeeDetails.DeleteCall(employee[0].Id);
            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, responseStatus);
            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public async Task DeleteCallNullTest()
        {
            // Arrange
            EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
            var Employee = new Mock<Employee>();
            // Act
            var responseStatus = await employeeDetails.DeleteCall(Employee.Object.Id);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, responseStatus);
            Assert.IsNotNull(responseStatus);
        }
    }
}