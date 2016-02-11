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
            return null;
        }

        public void ExecuteOrder(ProblemDescription description, int i, List<Command> commands)
        {
            var order = description.Orders[i];
            description.Orders.RemoveAt(i);
            var orderProductsAux = new Dictionary<int,int>(order.orderedProducts);

            var warehouseList = FindBestPath(order, description);

            foreach (int idWarehouse in warehouseList)
            {
                foreach(int idProd in order.orderedProducts.Keys) {
                    if (order.orderedProducts[idProd] > 0)
                    {
                        int ammountToRetrieve = Math.Min(order.orderedProducts[idProd], description.Warehouses[idWarehouse].heldProducts[idProd]);
                        description.Warehouses[idWarehouse].heldProducts[idProd] -= ammountToRetrieve; //remove from available on wh
                        order.orderedProducts[idProd] -= ammountToRetrieve; //remove from order

                        LoadCommand command = new LoadCommand();
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
                command.CustomerId = order.RealId;
                command.ProductId = idProd;
                command.ProductAmount = orderProductsAux[idProd];
                commands.Add(command);
                TurnsToNextAction += DistanceCalculator.CalculateDistance(NextPosition, description.Orders[order.RealId].position) + 1;
                NextPosition = description.Orders[order.RealId].position;
            }
            // TODO: Find best path
            // Add commands
            // Find warehouses
            // Remove products from warehouse(s)
            // Set NextPosition/TurnsToNextAction
        }

        private List<int> FindBestPath(Order order, ProblemDescription description)
        {
            List<int> warehouseList = new List<int>();
            

            return warehouseList;
        }
    }
}
