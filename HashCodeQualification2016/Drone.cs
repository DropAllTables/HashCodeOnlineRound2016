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

            // TODO: Find best path
            // Add commands
            // Find warehouses
            // Remove products from warehouse(s)
            // Set NextPosition/TurnsToNextAction
        }
    }
}
