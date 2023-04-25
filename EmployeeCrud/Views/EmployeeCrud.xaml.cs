﻿using EmployeeCrud.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmployeeCrud.Views
{
    /// <summary>
    /// Interaction logic for EmployeeCrud.xaml
    /// </summary>
    public partial class EmployeeCrud : Window
    {
        HttpClient client = new HttpClient();
        public EmployeeCrud()
        {
            client.BaseAddress = new Uri(ApiConstants.ApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiConstants.ApiToken);
            InitializeComponent();
            this.GetEmployees();
        }

        public async void GetEmployees()
        {
            try
            {
                lblMessage.Content = "";
                var response = await client.GetStringAsync("users");
                var employee = JsonConvert.DeserializeObject<List<Employee>>(response);
                dgEmployee.DataContext = employee;
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
        }

        public async void SaveEmployee(Employee employee)
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
                var jsonData = System.Text.Json.JsonSerializer.Serialize(requestParams);
                var buffer = Encoding.UTF8.GetBytes(jsonData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await client.PostAsync("users/", byteContent);
                this.GetEmployees();
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
        }

        public async void UpdateEmployee(Employee employee)
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
                var jsonData = System.Text.Json.JsonSerializer.Serialize(requestParams);
                var buffer = Encoding.UTF8.GetBytes(jsonData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await client.PutAsync("users/" + employee.Id, byteContent);
                this.GetEmployees();
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
        }

        public async void DeleteEmployee(int employeeId)
        {
            try
            {
                await client.DeleteAsync("users/" + employeeId);
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
            finally
            {
                this.GetEmployees();
            }
        }

        public void btnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = new Employee()
                {
                    Id = Convert.ToInt32(txtEmployeeId.Text),
                    Name = txtName.Text,
                    Email = txtEmail.Text,
                    Gender = txtGender.Text,
                    Status = txtStatus.Text
                };

                if (employee.Id == 0)
                {
                    this.SaveEmployee(employee);
                    lblMessage.Content = "Employee Saved";
                }
                else
                {
                    this.UpdateEmployee(employee);
                    lblMessage.Content = "Employee Updated";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
            finally
            {
                txtEmployeeId.Text = 0.ToString();
                txtName.Text = "";
                txtEmail.Text = "";
                txtGender.Text = "";
                txtStatus.Text = "";
            }

        }

        public void btnEditEmployee(object sender, RoutedEventArgs e)
        {
            try
            {
                Employee? employee = ((FrameworkElement)sender).DataContext as Employee;
                txtEmployeeId.Text = employee?.Id.ToString();
                txtName.Text = employee?.Name;
                txtEmail.Text = employee?.Email;
                txtGender.Text = employee?.Gender;
                txtStatus.Text = employee?.Status;
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
        }
        public void btnDeleteEmployee(object sender, RoutedEventArgs e)
        {
            try
            {
                Employee? employee = ((FrameworkElement)sender).DataContext as Employee;
                this.DeleteEmployee(employee.Id);
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
        }

    }
}