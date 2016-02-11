using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public struct Position : IEquatable<Position>
    {
        public int Row, Column;

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"[Position {Row}, {Column}]";
        }

        public override bool Equals(object obj)
        {
            var pos = obj as Position?;
            return pos != null &&
                Equals(pos.Value);
        }

        public bool Equals(Position position)
        {
            return Row == position.Row && Column == position.Column;
        }

        public override int GetHashCode()
        {
            return (Row << 16) ^ Column;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return Equals(p1, p2);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }
    }
}
