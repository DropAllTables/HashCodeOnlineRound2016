using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    class Drone
    {
        public int Id;

        Position NextPosition;
        int TurnsToNextAction;

        public Drone(int id, Position initialPosition)
        {
            Id = id;
            NextPosition = initialPosition;
        }

        public bool IsAvailable
            => TurnsToNextAction == 0;

        internal void ExecuteTurn(ProblemDescription description, List<Command> commands)
        {
            if (TurnsToNextAction > 0)
            {
                --TurnsToNextAction;
            } else
            {
                var bestOrder = GetBestOrder(description, NextPosition);

                if (bestOrder != null)
                {
                    ExecuteOrder(description, bestOrder.Value, commands);
                }
            }
        }

        public int? GetBestOrder(ProblemDescription description, Position nextPosition)
        {
            var orders = description.Orders
                .Select((order, orderId) => new { order, orderId })
                .OrderBy(item => GetScore(item.order, description, nextPosition)).ToList();
            return orders.FirstOrDefault()?.orderId;
        }

        private int GetScore(Order order, ProblemDescription description, Position nextPosition)
        {
            int score = 0;
            Position pos = nextPosition;
            foreach (var a in FindBestPath(order, description))
            {
                score += DistanceCalculator.CalculateDistance(pos, description.Warehouses[a].position);
                pos = description.Warehouses[a].position;
            }
            score += DistanceCalculator.CalculateDistance(pos, order.position);
            return score;
        }

        public void ExecuteOrder(ProblemDescription description, int i, List<Command> commands)
        {
            var order = description.Orders[i];
            description.Orders.RemoveAt(i);
            var orderProductsAux = new Dictionary<int,int>(order.orderedProducts);

            var warehouseList = FindBestPath(order, description);

            foreach (int idWarehouse in warehouseList)
            {
                foreach(int idProd in order.orderedProducts.Keys.ToList()) {
                    if (order.orderedProducts[idProd] == 0) continue;

                    if (idProd >= description.Warehouses[idWarehouse].heldProducts.Count) continue;
                    if (description.Warehouses[idWarehouse].heldProducts[idProd] > 0)
                    {
                        int ammountToRetrieve = Math.Min(order.orderedProducts[idProd], description.Warehouses[idWarehouse].heldProducts[idProd]);

                        description.Warehouses[idWarehouse].heldProducts[idProd] -= ammountToRetrieve; //remove from available on wh
                        order.orderedProducts[idProd] -= ammountToRetrieve; //remove from order

                        LoadCommand command = new LoadCommand();
                        command.DroneId = Id;
                        command.WarehouseId = idWarehouse;
                        command.ProductId = idProd;
                        command.ProductAmount = ammountToRetrieve;
                        commands.Add(command);                   
                    }
                    TurnsToNextAction += DistanceCalculator.CalculateDistance(NextPosition, description.Warehouses[idWarehouse].position) + 1;
                    NextPosition = description.Warehouses[idWarehouse].position;
                }
            }
            foreach(int idProd in orderProductsAux.Keys)
            {
                if (orderProductsAux[idProd] == 0) continue;
                var command = new DeliverCommand();
                command.DroneId = Id;
                command.CustomerId = order.RealId;
                command.ProductId = idProd;
                command.ProductAmount = orderProductsAux[idProd];
                commands.Add(command);
                TurnsToNextAction += DistanceCalculator.CalculateDistance(NextPosition, order.position) + 1;
                NextPosition = order.position;
            }
        }

        private List<int> FindBestPath(Order order, ProblemDescription description)
        {
            List<int> warehouseList = new List<int>();

            // FIXME: Do this better.
            // We may be able to find a single warehouse with everything.
            // So maybe try multiple alternatives

            Dictionary<int, int> missingItems = new Dictionary<int, int>(order.orderedProducts);

            List<Tuple<Warehouse, int>> warehouses = description.Warehouses
                .Select((warehouse, warehouseId) => Tuple.Create(warehouse, warehouseId))
                .ToList();

            var activePosition = NextPosition;
            for (;;)
            {
                int maxValue = int.MaxValue;
                int minIndex = -1;
                for (int i = 0; i < warehouses.Count; ++i)
                {
                    var warehouse = warehouses[i];
                    var distance = DistanceCalculator.CalculateSquareDistance(activePosition, warehouse.Item1.position);
                    if (distance < maxValue)
                    {
                        maxValue = distance;
                        minIndex = i;
                    }
                }
                
                var tuple = warehouses[minIndex];

                var heldProducts = tuple.Item1.heldProducts;
                bool anyMatch = false;

                foreach (var itemType in missingItems.Keys.ToList())
                {
                    if (missingItems[itemType] == 0)
                    {
                        continue;
                    }

                    if (itemType < heldProducts.Count && heldProducts[itemType] > 0)
                    {
                        var numProducts = Math.Min(heldProducts[itemType], missingItems[itemType]);

                        anyMatch = true;
                        missingItems[itemType] -= numProducts;
                        if (missingItems[itemType] == 0)
                        {
                            missingItems.Remove(itemType);
                        }
                    }
                }

                if (anyMatch)
                {
                    warehouseList.Add(tuple.Item2);
                    warehouses.Remove(tuple);
                    activePosition = tuple.Item1.position;
                    break;
                }

                if (missingItems.Count == 0)
                {
                    break;
                }
            }

            return warehouseList;
        }
    }
}
