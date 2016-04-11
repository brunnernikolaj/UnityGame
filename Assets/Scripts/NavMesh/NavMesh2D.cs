using Poly2Tri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class NavMesh2D
    {
        //private Stack<>

        public List<DelaunayTriangle> Triangles { get; private set; }

        public NavMesh2D(List<DelaunayTriangle> triangles)
        {
            Triangles = triangles;
        }

        /// <summary>
        /// Finds a path from startPoint to endPoint using the A* Algorithm 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns>A List with representing the path to walk</returns>
        public List<Vector2> PathFind(Vector2 startPoint, Vector2 endPoint)
        {
            var startTriangle = FindTriangle(startPoint);
            var endTriangle = FindTriangle(endPoint);

            if (startTriangle == null || endTriangle == null)
                return null;

            TrianglePath startNode = FindStartNode(startPoint, endPoint, startTriangle);

            var open = new List<TrianglePath> { startNode };
            var closed = new HashSet<TrianglePath>();

            while (open.Count > 0)
            {
                var cheapestNode = open.OrderBy(x => x.FCost).First();

                foreach (var item in FindNeighbourPoints2(cheapestNode.Point.ToTriPoint()))
                {
                    item.Parent = cheapestNode;

                    item.GCost = cheapestNode.GCost + Vector2.Distance(item.Point, cheapestNode.Point);
                    item.HCost = Vector2.Distance(item.Point, endPoint);

                    if (open.Contains(item))
                    {
                        continue;
                    }
                    if (closed.Contains(item))
                    {
                        continue;
                    }
                    open.Add(item);
                }
                open.Remove(cheapestNode);
                closed.Add(cheapestNode);

                if (endTriangle.Points.Contains(cheapestNode.Point.ToTriPoint()))
                {
                    return BuildPath(endPoint, cheapestNode);
                }
            }
            //var neighbourPoints = FindNeighbourPoints(startTriangle, next.Key);
            return null;

        }

        private static TrianglePath FindStartNode(Vector2 startPoint, Vector2 endPoint, DelaunayTriangle startTriangle)
        {
            var startPoints = startTriangle.Points.Select(x => new TrianglePath { Point = x.ToVector2() });

            TrianglePath startNode = new TrianglePath { Point = startPoint, GCost = 99999 };

            TrianglePath temp = new TrianglePath { GCost = 9999};

            foreach (var item in startPoints)
            {
                var gCost = Vector2.Distance(item.Point, startPoint);
                var hCost = Vector2.Distance(item.Point, endPoint);
                var fCost = gCost + hCost;

                if (temp.FCost > fCost)
                {
                    item.Parent = startNode;
                    item.GCost = gCost;
                    item.HCost = hCost;
                    temp = item;
                }
            }

            startNode = temp;

            return startNode;
        }

        private static List<Vector2> BuildPath(Vector2 endPoint, TrianglePath item)
        {
            var node = item;
            List<Vector2> path = new List<Vector2>();
            path.Add(endPoint);
            while (node != null)
            {
                path.Add(node.Point);
                node = node.Parent;
            }


            path.Reverse();

            int layerMask = 1 << 8;

            for (int i = 0; i < path.Count - 1; i++)
            {
                for (int k = i + 2; k < path.Count; k++)
                {
                    if (Physics2D.Linecast(path[i], path[k], layerMask).collider == null)
                    {
                        path.Remove(path[i + 1]);
                    }
                    else
                    {
                        break;
                    }
                }

            }
            path.Reverse();
            return path;
        }

        private IEnumerable<TrianglePath> FindNeighbourPoints2(TriangulationPoint startPoint)
        {
            foreach (var triangle in Triangles.Where(x => x.Points.Contains(startPoint)))
            {
                foreach (var point in triangle.Points)
                {
                    yield return new TrianglePath { Point = point.ToVector2() };
                }
            }
        }

        private IEnumerable<TrianglePath> FindNeighbourPoints(DelaunayTriangle triangle, Vector2 next)
        {

            foreach (var neighbour in triangle.Neighbors)
            {
                foreach (var point in neighbour.Points)
                {

                    yield return new TrianglePath { Point = point.ToVector2() };

                }
            }

        }

        public DelaunayTriangle FindTriangle(Vector2 position)
        {
            foreach (var triangle in Triangles)
            {
                if (PointInTriangle(position, triangle.Points[0].ToVector2(), triangle.Points[1].ToVector2(), triangle.Points[2].ToVector2()))
                    return triangle;
            }
            return null;
        }

        float sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (float)((p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y));
        }

        bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            bool b1, b2, b3;

            b1 = sign(pt, v1, v2) < 0.0f;
            b2 = sign(pt, v2, v3) < 0.0f;
            b3 = sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        // Return True if the point is in the polygon.
        private bool PointInPolygon(IntPoint[] polygon, float X, float Y)
        {
            // Get the angle between the point and the
            // first and last vertices.
            int max_point = polygon.Length - 1;
            float total_angle = GetAngle(
                polygon[max_point].X, polygon[max_point].Y,
                X, Y,
                polygon[0].X, polygon[0].Y);

            // Add the angles from the point
            // to each other pair of vertices.
            for (int i = 0; i < max_point; i++)
            {
                total_angle += GetAngle(
                    polygon[i].X, polygon[i].Y,
                    X, Y,
                    polygon[i + 1].X, polygon[i + 1].Y);
            }

            // The total angle should be 2 * PI or -2 * PI if
            // the point is in the polygon and close to zero
            // if the point is outside the polygon.
            return (Mathf.Abs(total_angle) > 0.000001);
        }

        private float GetAngle(float Ax, float Ay,
        float Bx, float By, float Cx, float Cy)
        {
            // Get the dot product.
            float dot_product = DotProduct(Ax, Ay, Bx, By, Cx, Cy);

            // Get the cross product.
            float cross_product = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy);

            // Calculate the angle.
            return Mathf.Atan2(cross_product, dot_product);
        }

        private float DotProduct(float Ax, float Ay,
        float Bx, float By, float Cx, float Cy)
        {
            // Get the vectors' coordinates.
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            // Calculate the dot product.
            return (BAx * BCx + BAy * BCy);
        }

        public float CrossProductLength(float Ax, float Ay,
        float Bx, float By, float Cx, float Cy)
        {
            // Get the vectors' coordinates.
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            // Calculate the Z coordinate of the cross product.
            return (BAx * BCy - BAy * BCx);
        }

        internal class Path
        {
            public Vector2 Position { get; set; }

            public Path Parent { get; set; }
        }



    }

    internal class TrianglePath : IEquatable<TrianglePath>
    {
        public float GCost { get; set; }

        public float HCost { get; set; }

        public float FCost { get { return GCost + HCost; } }

        public Vector2 Point { get; set; }

        public TrianglePath Parent { get; set; }

        public bool Equals(TrianglePath other)
        {
            return Point == other.Point;
        }
    }
}
