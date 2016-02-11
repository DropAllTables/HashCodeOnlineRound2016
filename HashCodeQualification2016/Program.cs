using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs = { "busy_day.in", "mother_of_all_warehouses.in", "redundancy.in" };
            foreach (var input in inputs)
            {

                var description = ProblemDescription.LoadFromFile(input);
                var commands = Solver.Execute(description);
                SolutionWriter.WriteToFile(commands, input + ".out");
            }
        }
    }
}
