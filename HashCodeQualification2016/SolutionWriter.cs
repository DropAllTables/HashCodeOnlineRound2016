using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public static class SolutionWriter
    {
        public static void WriteToFile(List<Command> commands, string path)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                file.WriteLine(commands.Count);
                foreach (Command command in commands)
                {
                    file.WriteLine(command.ToString());
                }
            }
        }
    }
}
