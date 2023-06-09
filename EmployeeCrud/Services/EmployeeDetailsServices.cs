﻿using EmployeeCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json.Nodes;

namespace EmployeeCrud.Services
{
    public class EmployeeDetailsServices : IEmployeeDetailsServices
    {
        HttpClient client = new HttpClient();
        public EmployeeDetailsServices()
        {
            ApiConstants apiConstants = new ApiConstants();
            client.BaseAddress = new Uri(apiConstants.GetApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiConstants.GetApiToken);
        }
        public async Task<string> GetCall(string url)
        {
            try
            {
                var response = await client.GetStringAsync("users");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostandPutCall(Employee employee)
        {
            try
            {
                JsonObject requestParams = new JsonObject
                    {
                    { "name", employee.Name },
                    { "email", employee.Email },
                    { "gender", employee.Gender },
                    { "status", employee.Status }
                    };
                if (employee.Id == 0)
                {
                    var jsonData = System.Text.Json.JsonSerializer.Serialize(requestParams);
                    var buffer = Encoding.UTF8.GetBytes(jsonData);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync("users/", byteContent);
                    return response;
                }
                else
                {
                    var jsonData = System.Text.Json.JsonSerializer.Serialize(requestParams);
                    var buffer = Encoding.UTF8.GetBytes(jsonData);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PutAsync("users/" + employee.Id, byteContent);
                    return response;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpStatusCode> DeleteCall(int employeeId)
        {
            HttpResponseMessage response = await client.DeleteAsync("users/" + employeeId);
            return response.StatusCode;
        }
    }
}
