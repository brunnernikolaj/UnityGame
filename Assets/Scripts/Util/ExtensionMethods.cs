﻿using Poly2Tri;
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

        public static Vector2 ToVector2(this IntPoint source)
        {
            return new Vector2((float)source.X, (float)source.Y);
        }

        public static IntPoint ToIntPoint(this Vector2 source)
        {
            return new IntPoint(source.x, source.y);
        }

        public static TriangulationPoint ToTriPoint(this Vector2 source)
        {
            return new TriangulationPoint(Math.Round(source.x,1), Math.Round(source.y, 1));
        }

        public static float AngleBetween(this Transform source, Vector3 other)
        {
            var relative = other - source.position;
            return Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        }

        //public static Triangle ToTriangle(this DelaunayTriangle source)
        //{

        //    var points = source.Points.Select(x => x.ToVector2());
        //    var neighbours = source.Neighbors.s

        //    return new Triangle(points, source.Neighbors);
        //}
    }
}
