using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices.Exceptions
{
    public class SignUpFailedException :Exception
    {
        public SignUpFailedException(string message) : base(message) { }
    }
}
