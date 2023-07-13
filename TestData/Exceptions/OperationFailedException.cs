using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Exceptions
{
    public class OperationFailedException : Exception
    {
        public object? Obj { get; }

        public OperationFailedException(string message) : base(message) { }
        public OperationFailedException(object obj,string message) : base(message) 
        { 
            Obj = obj;
        }
    }
}
