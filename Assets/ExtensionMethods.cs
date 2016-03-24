using Poly2Tri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public static class ExtensionMethods
    {
        public static Vector2 ToVector2(this TriangulationPoint source)
        {
            return new Vector2((float)source.X, (float)source.Y);
        }

        public static TriangulationPoint ToTriPoint(this Vector2 source)
        {
            return new TriangulationPoint(source.x, source.y);
        }

        //public static Triangle ToTriangle(this DelaunayTriangle source)
        //{

        //    var points = source.Points.Select(x => x.ToVector2());
        //    var neighbours = source.Neighbors.s

        //    return new Triangle(points, source.Neighbors);
        //}
    }
}
