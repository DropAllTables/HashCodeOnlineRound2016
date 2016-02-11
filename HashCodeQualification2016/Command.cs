using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public abstract class Command
    {
        public abstract string ToString(int droneID);
    }

    public class WaitCommand : Command
    {
        public int Turns = 0;
        public override string ToString(int droneID) 
        {
            return droneID + " W " + Turns;
        }
    }

    public class LoadCommand : Command
    {
        public int WarehouseId;
        public int ProductId;
        public int ProductAmount;

        public override string ToString(int droneID)
        {
            return droneID + " L " + WarehouseId + " " + ProductId + " " + ProductAmount;
        }
    }

    public class UnloadCommand : Command
    {
        public int WarehouseId;
        public int ProductId;
        public int ProductAmount;

        public override string ToString(int droneID)
        {
            return droneID + " U " + WarehouseId + " " + ProductId + " " + ProductAmount;
        }
    }

    public class DeliverCommand : Command
    {
        public int CustomerId;
        public int ProductId;
        public int ProductAmount;

        public override string ToString(int droneID)
        {
            return droneID + " D " + CustomerId + " " + ProductId + " " + ProductAmount;
        }
    }
}
