using Poly2Tri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// This class encapsulate the A* pathfinding algorithm working on a navmesh
    /// </summary>
    class NavMesh2D
    {
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

                foreach (var item in FindNeighbourPoints(cheapestNode.Point.ToTriPoint()))
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
            return null;
        }

        /// <summary>
        /// Finds the best start node on the first triangle 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="startTriangle"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Builds a List of the path to walk
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="item"></param>
        /// <returns></returns>
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
            SmoothPath(path);
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Try and smooth path by CircleCasting from path[n] to path[n + 2]
        /// </summary>
        /// <param name="path"></param>
        private static void SmoothPath(List<Vector2> path)
        {
            //The layer mask for our obstacle layer
            int layerMask = 1 << 8;

            for (int i = 0; i < path.Count - 1; i++)
            {
                for (int k = i + 2; k < path.Count; k++)
                {
                    Vector3 relative = path[k] - path[i];
                    Vector3 movementNormal = Vector3.Normalize(relative);
                    var direction = new Vector2(movementNormal.x, movementNormal.y);

                    var col = Physics2D.CircleCast(path[i], 1f, direction, Vector3.Distance(path[i], path[k]), layerMask);

                    if (col.transform == null)
                    {
                        path.Remove(path[i + 1]);
                    }
                    else
                    {
                        break;
                    }
                }

            }
        }

        private IEnumerable<TrianglePath> FindNeighbourPoints(TriangulationPoint startPoint)
        {
            foreach (var triangle in Triangles.Where(x => x.Points.Contains(startPoint)))
            {
                foreach (var point in triangle.Points)
                {
                    yield return new TrianglePath { Point = point.ToVector2() };
                }
            }
        }

        /// <summary>
        /// Find the triangle that position is inside
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find if pt is inside triangle
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            bool b1, b2, b3;

            b1 = sign(pt, v1, v2) < 0.0f;
            b2 = sign(pt, v2, v3) < 0.0f;
            b3 = sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }
    }

    /// <summary>
    /// Class used for A* pathfinding
    /// </summary>
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
