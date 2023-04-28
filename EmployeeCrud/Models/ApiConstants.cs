using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCrud.Models
{
    public class ApiConstants
    {
        private const string ApiUrl = "https://gorest.co.in/public/v2/";

        private const string ApiToken = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";

        public string GetApiUrl
        {
            get
            {
                return ApiUrl;
            }
        }
        public string GetApiToken
        {
            get
            {
                return ApiToken;
            }
        }
    }
}
