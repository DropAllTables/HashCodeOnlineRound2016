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
            string input = "mother_of_all_warehouses.in";

            var description = ProblemDescription.LoadFromFile(input);
            var commands = Solver.Execute(description);
            SolutionWriter.WriteToFile(commands, input + ".out");
        }
    }
}
