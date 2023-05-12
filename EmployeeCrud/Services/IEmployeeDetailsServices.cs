using EmployeeCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCrud.Services
{
    public interface IEmployeeDetailsServices
    {
        public Task<string> GetCall(string url);
        public Task<HttpResponseMessage> PostandPutCall(Employee employee);
        public Task<HttpStatusCode> DeleteCall(int employeeId);
    }
}
