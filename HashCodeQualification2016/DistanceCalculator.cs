﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeQualification2016
{
    public static class DistanceCalculator
    {
        public static int CalculateSquareDistance(Position p1, Position p2)
        {
            int rowDiff = p1.Row - p2.Row;
            int colDiff = p1.Column - p2.Column;

            return rowDiff * rowDiff + colDiff * colDiff;
        }
        public static int CalculateDistance(Position p1, Position p2)
        {
            return (int)Math.Ceiling(Math.Sqrt(CalculateSquareDistance(p1, p2)));
        }
    }
}
