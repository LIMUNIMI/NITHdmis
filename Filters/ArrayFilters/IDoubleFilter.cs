using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NITHdmis.Utils.ValueFilters
{
    public interface IDoubleArrayFilter
    {
        void Push(double[] value);
        double[] Pull();
    }
}
