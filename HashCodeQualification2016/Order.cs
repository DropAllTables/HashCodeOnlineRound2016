using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public class Order
    {
        public int RealId;
        public Position position;
        public Dictionary<int, int> orderedProducts = new Dictionary<int, int>();

        public Order Copy()
        {
            return new Order
            {
                position = position,
                orderedProducts = new Dictionary<int, int>(orderedProducts)
            };
        }
    }
}
