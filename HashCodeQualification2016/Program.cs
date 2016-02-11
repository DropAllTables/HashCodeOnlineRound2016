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
            string input = "busy_day.in";

            var description = ProblemDescription.LoadFromFile(input);
            var commands = Solver.Execute(description);
            SolutionWriter.WriteToFile(commands, input + ".out");
        }
    }
}
