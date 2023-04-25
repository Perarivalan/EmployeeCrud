using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeCrud.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models;
using NuGet.Frameworks;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace EmployeeCrud.Views.Tests
{
    [TestClass]
    public class EmployeeCrudTests
    {
        [TestMethod]
        public async Task GetEmployeesTest()
        {
            // Arrange
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiConstants.ApiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiConstants.ApiToken);
            // Act
            var response = await client.GetStringAsync("users");
            // Assert
            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public async Task SaveEmployeeTest()
        {
            // Arrange
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiConstants.ApiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiConstants.ApiToken);
            var mockEmployee = GetMockEmployees();
            JsonObject requestMockParams = new JsonObject
            {
                { "name", mockEmployee[1].Name },
                { "email", mockEmployee[1].Email },
                { "gender", mockEmployee[1].Gender },
                { "status", mockEmployee[1].Status }
            };
            // Act
            var jsonData = System.Text.Json.JsonSerializer.Serialize(requestMockParams);
            var buffer = Encoding.UTF8.GetBytes(jsonData);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("users/", byteContent);
            // Assert
            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public async Task UpdateEmployeeTest()
        {
            // Arrange
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiConstants.ApiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiConstants.ApiToken);
            var mockEmployee = GetMockEmployees();
            JsonObject requestMockParams = new JsonObject
            {
                { "name", mockEmployee[3].Name },
                { "email", mockEmployee[3].Email },
                { "gender", mockEmployee[3].Gender },
                { "status", mockEmployee[3].Status }
            };
            // Act
            var jsonData = System.Text.Json.JsonSerializer.Serialize(requestMockParams);
            var buffer = Encoding.UTF8.GetBytes(jsonData);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PutAsync("users/" + mockEmployee[1].Id, byteContent);
            // Assert
            Assert.IsNotNull(response);
        }


        [TestMethod()]
        public async Task DeleteEmployeeTest()
        {
            // Arrange
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiConstants.ApiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiConstants.ApiToken);
            var mockProducts = GetMockEmployees();
            // Act
            var response = await client.DeleteAsync("users/" + mockProducts[2].Id);
            // Assert
            Assert.IsNotNull(response);
        }

        private List<Employee> GetMockEmployees()
        {
            var testProducts = new List<Employee>();
            testProducts.Add(new Employee { Id = 1, Name = "mock1", Email="test@gmail.com", Gender="male", Status="active" });
            testProducts.Add(new Employee { Id = 2, Name = "Demo2", Email = "new@gmail.com", Gender = "female", Status = "inactive" });
            testProducts.Add(new Employee { Id = 3, Name = "mock3", Email = "mock@gmail.com", Gender = "male", Status = "inactive" });
            testProducts.Add(new Employee { Id = 4, Name = "Demo4", Email = "mocktest@gmail.com", Gender = "male", Status = "active" });
            return testProducts;
        }
    }
}