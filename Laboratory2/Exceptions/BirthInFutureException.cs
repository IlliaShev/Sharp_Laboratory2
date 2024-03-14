using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory2.Exceptions
{
    class BirthInFutureException : Exception
    {
        public BirthInFutureException(string message) : base(message) 
        {

        }
    }
}
