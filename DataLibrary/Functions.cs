using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public enum FRawEnum
    {
        FRawLinear,
        FRawCubic,
        FRawRandom
    }

    public delegate double FRaw(double x);
}
