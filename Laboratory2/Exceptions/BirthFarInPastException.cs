using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory2.Exceptions
{
    class BirthFarInPastException : Exception
    {
        public BirthFarInPastException(string message) : base(message)
        {

        }
    }
}
