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

            var warehouseList = FindBestPath(order, description);

            foreach (int idWarehouse in warehouseList)
            {
                foreach(int idProd in order.orderedProducts.Keys) {
                    if (order.orderedProducts[idProd] > 0)
                    {
                        LoadCommand command = new LoadCommand();
                        command.WarehouseId = idWarehouse;
                        command.ProductId = idProd;
                        command.ProductAmount = Math.Min(0, 1);
                        commands.Add(command);
                    }
                }
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
