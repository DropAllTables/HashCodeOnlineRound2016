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
            var orders = description.Orders.OrderBy(order =>
            {
                int score = 0;
                Position pos = nextPosition;
                foreach (var a in FindBestPath(order, description))
                {
                    score += DistanceCalculator.CalculateDistance(pos, description.Warehouses[a].position);
                    pos = description.Warehouses[a].position;
                }
                score += DistanceCalculator.CalculateDistance(pos, description.Orders[order.RealId].position);
                return score;
            }).ToList();
            var firstOrder = orders.FirstOrDefault();
            return firstOrder?.RealId;
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
                var command = new DeliverCommand();
                command.DroneId = Id;
                command.CustomerId = order.RealId;
                command.ProductId = idProd;
                command.ProductAmount = orderProductsAux[idProd];
                commands.Add(command);
                TurnsToNextAction += DistanceCalculator.CalculateDistance(NextPosition, description.Orders[order.RealId].position) + 1;
                NextPosition = description.Orders[order.RealId].position;
            }
        }

        private List<int> FindBestPath(Order order, ProblemDescription description)
        {
            List<int> warehouseList = new List<int>();

            // FIXME: Do this better.
            // We may be able to find a single warehouse with everything.
            // So maybe try multiple alternatives

            Dictionary<int, int> missingItems = new Dictionary<int, int>(order.orderedProducts);

            var activePosition = NextPosition;
            for (;;)
            {
                foreach (var warehouse in description.Warehouses
                    .Select((warehouse, warehouseId) => new { warehouse, warehouseId }) // Get warehouses and IDs
                    .Where(item => !warehouseList.Contains(item.warehouseId)) // That we haven't visited before
                    .OrderBy(item => DistanceCalculator.CalculateDistance(activePosition, item.warehouse.position)) // Closest ones first
                    )
                {
                    var heldProducts = warehouse.warehouse.heldProducts;
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
                        }
                    }

                    if (anyMatch)
                    {
                        warehouseList.Add(warehouse.warehouseId);
                        activePosition = warehouse.warehouse.position;
                        break;
                    }
                }

                if (missingItems.All(item => item.Value == 0))
                {
                    break;
                }
            }

            return warehouseList;
        }
    }
}
