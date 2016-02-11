using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public class ProblemDescription
    {
        public int NumRows, NumCols;
        public int NumDrones;
        public int Deadline;
        public int MaximumLoad;
        public List<int> ProductWeights;
        public List<Warehouse> Warehouses = new List<Warehouse>();
        public List<Order> Orders = new List<Order>();

        public ProblemDescription Copy()
        {
            return new ProblemDescription
            {
                NumRows = NumRows,
                NumCols = NumCols,
                Deadline = Deadline,
                MaximumLoad = MaximumLoad,
                ProductWeights = new List<int>(ProductWeights),
                Warehouses = Warehouses.Select(_ => _.Copy()).ToList(),
                Orders = Orders.Select(_ => _.Copy()).ToList()
            };
        }

        public static ProblemDescription LoadFromFile(string path)
        {
            var description = new ProblemDescription();

            using (var reader = File.OpenText(path))
            {
                var parametersLine = reader.ReadLine();

                var parameters = parametersLine.Split(' ');

                description.NumRows = int.Parse(parameters[0]);
                description.NumCols = int.Parse(parameters[1]);

                description.NumDrones = int.Parse(parameters[2]);
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
                int orderId = 0;
                for (int i = 0; i < numOrders; ++i)
                {
                    var locationLine = reader.ReadLine();
                    var location = locationLine.Split(' ');

                    var order = new Order();
                    order.RealId = orderId++;
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

                    BreakAndAddOrder(description, order);
                }
            }

            return description;
        }

        private static void BreakAndAddOrder(ProblemDescription description, Order order)
        {
            int maxPayload = description.MaximumLoad;

            while (GetOrderLoad(description, order) > maxPayload)
            {
                var newOrder = new Order();
                newOrder.RealId = order.RealId;
                newOrder.position = order.position;
                int newLoad = 0;

                for (;;)
                {
                    int heaviestItemSoFar = -1;
                    int bestWeight = int.MinValue;
                    foreach (var item in order.orderedProducts)
                    {
                        if (item.Value == 0) continue;

                        int weight = description.ProductWeights[item.Key];
                        if (newLoad + weight < maxPayload && weight > bestWeight)
                        {
                            heaviestItemSoFar = item.Key;
                            bestWeight = weight;
                        }
                    }

                    if (heaviestItemSoFar < 0)
                    {
                        break;
                    }

                    int numItems = (maxPayload - newLoad) / bestWeight;
                    numItems = Math.Min(numItems, order.orderedProducts[heaviestItemSoFar]);
                    order.orderedProducts[heaviestItemSoFar] -= numItems;
                    newOrder.orderedProducts[heaviestItemSoFar] = numItems;
                    newLoad += numItems * bestWeight;
                }

                description.Orders.Add(newOrder);
            }
            description.Orders.Add(order);
        }

        private static int GetOrderLoad(ProblemDescription description, Order order)
        {
            int total = 0;
            foreach (var item in order.orderedProducts)
            {
                var type = item.Key;
                var quantity = item.Value;
                total += description.ProductWeights[type] * quantity;
            }

            return total;
        }
    }
}
