using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public class Warehouse
    {
        public Position position;
        public List<int> heldProducts;

        public Warehouse Copy()
        {
            return new Warehouse
            {
                position = position,
                heldProducts = new List<int>(heldProducts)
            };
        }
    }
}
