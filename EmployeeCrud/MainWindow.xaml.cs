using EmployeeCrud.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json.Nodes;
using System.Net.Http.Json;

namespace EmployeeCrud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        public MainWindow()
        {
            client.BaseAddress = new Uri("https://gorest.co.in/public/v2/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "a3747811879458ac7dadbf728d15cfa456c957fbeeb2156afb6f33fd767e4030");
            InitializeComponent();
            this.GetEmployees();
        }

        public void btnLoadEmployees_Click(object sender, RoutedEventArgs e)
        {
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
                JsonContent content = JsonContent.Create(jsonData);
                await client.PostAsync("users/", content);
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
                JsonContent content = JsonContent.Create(jsonData);
                await client.PutAsync("users/" + employee.Id, content);
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
