using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public class Order
    {
        public Position position;
        public Dictionary<int, int> orderedProducts = new Dictionary<int, int>();
    }
}
