using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public abstract class Command
    {
        public int DroneId;
        public abstract string ToString();
    }

    public class WaitCommand : Command
    {
        public int Turns = 0;
        public override string ToString() 
        {
            return DroneId + " W " + Turns;
        }
    }

    public class LoadCommand : Command
    {
        public int WarehouseId;
        public int ProductId;
        public int ProductAmount;

        public override string ToString()
        {
            return DroneId + " L " + WarehouseId + " " + ProductId + " " + ProductAmount;
        }
    }

    public class UnloadCommand : Command
    {
        public int WarehouseId;
        public int ProductId;
        public int ProductAmount;

        public override string ToString()
        {
            return DroneId + " U " + WarehouseId + " " + ProductId + " " + ProductAmount;
        }
    }

    public class DeliverCommand : Command
    {
        public int CustomerId;
        public int ProductId;
        public int ProductAmount;

        public override string ToString()
        {
            return DroneId + " D " + CustomerId + " " + ProductId + " " + ProductAmount;
        }
    }
}
