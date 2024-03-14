using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory2.Exceptions
{
    class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message) : base(message) { }
    }
}
