using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices.ServiceConstants
{
    public static class CompanyInfo
    {
        public readonly static string CompanyEmail = Environment.GetEnvironmentVariable("CompanyEmail", EnvironmentVariableTarget.Machine);
        public readonly static string CompanyPassword = Environment.GetEnvironmentVariable("CompanyPassword", EnvironmentVariableTarget.Machine);
    }
}
