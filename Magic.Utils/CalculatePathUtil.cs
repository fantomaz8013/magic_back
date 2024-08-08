using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Utils
{
    public static class CalculatePathUtil
    {
        public static bool IsNeighboringPoint(int x1, int y1, int x2, int y2)
        {
            if (Math.Abs(x1 - x2) > 1)
            {
                return false;
            }
            if (Math.Abs(y1 - y2) > 1)
            {
                return false;
            }
            return true;
        }

    }
}
