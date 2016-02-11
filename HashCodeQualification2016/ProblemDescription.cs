using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    class ProblemDescription
    {
        public int NumRows, NumCols;
        public int Deadline;
        public int MaximumLoad;
        public List<int> ProductWeights;
        public List<Warehouse> Warehouses = new List<Warehouse>();
        public List<Order> Orders = new List<Order>();

        public static ProblemDescription LoadFromFile(string path)
        {
            var description = new ProblemDescription();

            using (var reader = File.OpenText(path))
            {
                var parametersLine = reader.ReadLine();

                var parameters = parametersLine.Split(' ');

                description.NumRows = int.Parse(parameters[0]);
                description.NumCols = int.Parse(parameters[1]);

                int numDrones = int.Parse(parameters[2]);
                description.Deadline = int.Parse(parameters[3]);
                description.MaximumLoad = int.Parse(parameters[4]);

                var numProducts = int.Parse(reader.ReadLine());

                description.ProductWeights = reader.ReadLine()
                    .Split(' ')
                    .Select(int.Parse)
                    .ToList();

                int numWarehouses = int.Parse(reader.ReadLine());
                for (int i = 0; i < numWarehouses; ++i)
                {
                    var locationLine = reader.ReadLine();
                    var location = locationLine.Split(' ');

                    var warehouse = new Warehouse();
                    warehouse.position = new Position(int.Parse(location[0]),
                        int.Parse(location[1]));
                    warehouse.heldProducts = reader.ReadLine()
                        .Split(' ')
                        .Select(int.Parse)
                        .ToList();

                    description.Warehouses.Add(warehouse);
                }

                int numOrders = int.Parse(reader.ReadLine());
                for (int i = 0; i < numOrders; ++i)
                {
                    var locationLine = reader.ReadLine();
                    var location = locationLine.Split(' ');

                    var order = new Order();
                    order.position = new Position(int.Parse(location[0]),
                        int.Parse(location[1]));

                    int numProductTypes = int.Parse(reader.ReadLine());
                    var orderInfoLine = reader.ReadLine();
                    var orderInfoItems = orderInfoLine.Split(' ');
                    for (int j = 0; j < numProductTypes; ++j)
                    {
                        var productType = int.Parse(orderInfoItems[j]);

                        if (order.orderedProducts.ContainsKey(productType))
                        {
                            order.orderedProducts[productType]++;
                        } else
                        {
                            order.orderedProducts[productType] = 1;
                        }
                    }

                    description.Orders.Add(order);
                }
            }

            return description;
        }
    }
}
