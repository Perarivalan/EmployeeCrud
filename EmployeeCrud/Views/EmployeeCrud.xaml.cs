using EmployeeCrud.Models;
using EmployeeCrud.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
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
            ApiConstants apiConstants = new ApiConstants(); 
            client.BaseAddress = new Uri(apiConstants.GetApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiConstants.GetApiToken);
            InitializeComponent();
            this.GetEmployees();
        }

        public async void GetEmployees()
        {
            try
            {
                txtMessage.Text = "";
                EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
                var response = await employeeDetails.GetCall("users");
                var employee = JsonConvert.DeserializeObject<List<Employee>>(response);
                dgEmployee.DataContext = employee;
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
        }

        public async void SaveEmployee(Employee employee)
        {
            try
            {

                EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
                var response = await employeeDetails.PostandPutCall(employee);
                this.GetEmployees();
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
        }

        public async void UpdateEmployee(Employee employee)
        {
            try
            {

                EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
                var response = await employeeDetails.PostandPutCall(employee);
                this.GetEmployees();
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
        }

        public async void DeleteEmployee(int employeeId)
        {
            try
            {

                EmployeeDetailsAPI employeeDetails = new EmployeeDetailsAPI();
                var response = await employeeDetails.DeleteCall(employeeId);
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
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
                if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtGender.Text) && !string.IsNullOrEmpty(txtStatus.Text)) 
                {
                    if (ValidateRule(txtName.Text, txtEmail.Text, txtGender.Text, txtStatus.Text))
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
                            txtMessage.Text = "Employee Saved";
                        }
                        else
                        {
                            this.UpdateEmployee(employee);
                            txtMessage.Text = "Employee Updated";
                        }
                    }
                    else
                    {
                        txtMessage.Text = "Please enter valid Username, Email, Gender and Status to save.";
                    }
                }
                else
                {
                    txtMessage.Text = "All fields are Mandatory to save.";
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
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
                txtMessage.Text = ex.Message;
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
                txtMessage.Text = ex.Message;
            }
        }

        public bool ValidateRule(string User, string Email, string Gender, string Status)
        {
            Regex regexUser = new Regex(@"^[A-Za-z0-9 !@#$%^&]{3,25}$");
            Regex regexEmail = new Regex(@"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$");
            Regex regexGender = new Regex(@"^[A-Za-z0-9!@#$%^&]{4,6}$");
            Regex regexStatus = new Regex(@"^[A-Za-z0-9!@#$%^&]{6,8}$");
            if (!regexUser.IsMatch(User) || !regexEmail.IsMatch(Email) || !regexGender.IsMatch(Gender) || !regexStatus.IsMatch(Status))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
