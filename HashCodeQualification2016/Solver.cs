using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public static class Solver
    {
        public static List<Command> Execute(ProblemDescription description)
        {
            var commands = new List<Command>();
            var drones = new List<Drone>();
            for (int i = 0; i < description.NumDrones; ++i)
            {
                drones.Add(new Drone(i, description.Warehouses[0].position));
            }

            for (int turnId = 0; turnId < description.Deadline; ++turnId)
            {
                ExecuteTurn(description, drones, commands);
            }

            return commands;
        }

        static void ExecuteTurn(ProblemDescription description, List<Drone> drones, List<Command> commands)
        {
            foreach (var drone in drones)
            {
                drone.ExecuteTurn(description, commands);
            }
        }
    }
}
