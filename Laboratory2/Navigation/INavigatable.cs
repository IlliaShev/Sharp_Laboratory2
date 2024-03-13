using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory2.Navigation
{
    internal interface INavigatable<TObject> where TObject : Enum
    {
        TObject ViewType { get; }
    }
}
