using Poly2Tri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class NavMeshGenerator
    {
        public Color Color { get; set; }

        public Mesh Mesh { get; private set; }

        public int ActorSize { get; set; }

        public NavMeshGenerator()
        {
            Color = Color.blue;
        }

        public NavMesh2D Generate(IEnumerable<IEnumerable<Vector3>> walkable, IEnumerable<IEnumerable<Vector3>> blocked)
        {
            var walkablePoly = walkable.Select(list => list.Select(vector => new IntPoint(vector.x, vector.y)).ToList()).ToList();
            var obstaclesPolys = blocked.Select(list => list.Select(vector => new IntPoint(vector.x, vector.y)).ToList());

            walkablePoly = MergeWalkable(walkablePoly);

            obstaclesPolys = ExpandObstacles(obstaclesPolys);

            List<List<IntPoint>> walkableArea = SubtactPolys(walkablePoly, obstaclesPolys);

            //var walkable2 = new List<List<IntPoint>>
            //{
            //    walkableArea
            //    .Select(x => x.Select(y => Vector2.Scale(y.ToVector2(), new Vector2(0.1f, 0.1f)).ToIntPoint()))
            //};

            List<List<Vector3>> walkableArea2 = new List<List<Vector3>>();

            for (int i = 0; i < walkableArea.Count; i++)
            {
                List<Vector3> temp = new List<Vector3>();
                for (int k = 0; k < walkableArea[i].Count; k++)
                {
                    temp.Add(new Vector3 (walkableArea[i][k].X * 0.1f, walkableArea[i][k].Y * 0.1f));
                }
                walkableArea2.Add(temp);
            }

            Polygon poly = Triangulate(walkableArea);

            foreach (var tri in poly.Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    tri.Points[i] = new TriangulationPoint(Math.Round( tri.Points[i].X * 0.1f, 1), Math.Round(tri.Points[i].Y * 0.1f, 1));
                }
            }

            BuildMesh(walkableArea2, poly);

            return BuildTriangleMap(poly.Triangles);
        }

        private List<List<IntPoint>> MergeWalkable(List<List<IntPoint>> walkablePoly)
        {
            var clipper = new Clipper();
            var result = new List<List<IntPoint>>();
            clipper.AddPaths(walkablePoly, PolyType.ptClip, true);
            clipper.Execute(ClipType.ctUnion, result, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            return result;
        }

        /// <summary>
        /// Connects each triangle to its neighbours
        /// </summary>
        /// <param name="triangles"></param>
        /// <returns></returns>
        private NavMesh2D BuildTriangleMap(IList<DelaunayTriangle> triangles)
        {
            

            Dictionary<DelaunayTriangle, int> triangleIds = new Dictionary<DelaunayTriangle, int>();
            Dictionary<TriangulationPoint, int> vertexIds = new Dictionary<TriangulationPoint, int>();
            List<TriangulationPoint> vertices = new List<TriangulationPoint>();

            // give every triangle and vertex a unique ID
            foreach (var triangle in triangles)
            {
                triangleIds.Add(triangle, triangleIds.Count);
                foreach (var vertex in triangle.Points)
                {
                    if (!vertexIds.ContainsKey(vertex))
                    {
                        vertexIds.Add(vertex, vertexIds.Count);
                        vertices.Add(vertex);
                    }
                }
            }

            //Build a dictionary of edges pointing to triangles that contain them    
            Dictionary<Edge, List<DelaunayTriangle>> edgeToTriangles = new Dictionary<Edge, List<DelaunayTriangle>>();
            foreach (var triangle in triangles)
            {
                var edges = FindEdges(triangle);
                foreach (var edge in edges)
                {
                    if (!edgeToTriangles.ContainsKey(edge))
                    {
                        edgeToTriangles.Add(edge, new List<DelaunayTriangle> { triangle });
                    }
                    else
                    {
                        edgeToTriangles[edge].Add(triangle);
                    }
                }
            }

            //Connect neighbours that share an edge
            foreach (var triangle in triangles)
            {
                var edges = FindEdges(triangle);

                int index = 0;

                foreach (var edge in edges)
                {                    
                    foreach (var triangle2 in edgeToTriangles[edge])
                    {
                        if (triangle2.IsInterior)
                        {
                            triangle.Neighbors[index] = triangle2;                          
                        }                       
                    }
                    index++;
                }
            }

            return new NavMesh2D(triangles.ToList());
        }

        /// <summary>
        /// Finds the edges of a triangle
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private List<Edge> FindEdges(DelaunayTriangle triangle)
        {
            List<Edge> result = new List<Edge>();
            var v1 = triangle.Points[0];
            var v2 = triangle.Points[1];
            var v3 = triangle.Points[2];
            result.Add(new Edge(v1, v2));
            result.Add(new Edge(v2, v3));
            result.Add(new Edge(v3, v1));
            return result;
        }

        /// <summary>
        /// Builds a mesh that unity can work with
        /// </summary>
        /// <param name="walkableArea"></param>
        /// <param name="poly"></param>
        /// <returns></returns>
        private Mesh BuildMesh(List<List<Vector3>> walkableArea, Polygon poly)
        {
            var verts = walkableArea
                            .SelectMany(x => x)
                            .ToList();

            //Dictionary With vertices as keys and their index as value
            var dict = verts
                .Select((vert, index) => new { key = vert, value = index })
                .ToDictionary(k => new Vector3(k.key.x,k.key.y,0), v => v.value);

            var triangles = poly.Triangles
                .SelectMany(x => x.Points)
                .Select(y => dict.Where(x => new Vector3(x.Key.x, x.Key.y, 0) == new Vector3((float)y.X, (float)y.Y, 0)).FirstOrDefault().Value)
                .ToArray();


            //foreach (var item in poly.Triangles.SelectMany(x => x.Points))
            //{
            //    var vector = new Vector3((float)item.X, (float)item.Y, 0);
            //    var it = dict.Where(x => new Vector3(x.Key.x,x.Key.y,0) == vector).First();
            //    Debug.Log("hi");
            //}


            var colors = verts
                .Select(x => Color)
                .ToArray();

            // Create the mesh
            Mesh msh = new Mesh();

            msh.vertices = verts.ToArray();
            msh.colors = colors;
            msh.triangles = triangles;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            Mesh = msh;

            return msh;
        }

        private Polygon Triangulate(List<List<IntPoint>> walkableArea)
        {
            //Obstacles
            var clockwisePolygons = walkableArea.Where(x => IsClockwisePolygon(x.ToArray())).ToList();
            //Walkable
            var counterClockwisePolygons = walkableArea.Except(clockwisePolygons).ToList();

            Polygon poly = null;
            foreach (var polygon in counterClockwisePolygons)
            {
                poly = new Polygon(polygon.Select(x => new PolygonPoint(x.X, x.Y)));

                foreach (var hole in clockwisePolygons)
                {
                    poly.AddHole(new Polygon(hole.Select(x => new PolygonPoint(x.X, x.Y))));
                }
                P2T.Triangulate(poly);
            }

            return poly;
        }

        private static List<List<IntPoint>> SubtactPolys(List<List<IntPoint>> walkablePoly, IEnumerable<List<IntPoint>> obstaclesPolys)
        {
            var clipper = new Clipper();
            var result = walkablePoly;

            foreach (var obstacal in obstaclesPolys)
            {
                clipper = new Clipper();
                clipper.AddPaths(result, PolyType.ptSubject, true);
                clipper.AddPath(obstacal, PolyType.ptClip, true);

                result = new List<List<IntPoint>>();

                clipper.Execute(ClipType.ctDifference, result);
            }

            return result;
        }

        private IEnumerable<List<IntPoint>> ExpandObstacles(IEnumerable<List<IntPoint>> obstaclesPolys)
        {
            ClipperOffset offset = new ClipperOffset();
            var offsetContainer = new List<List<IntPoint>>();
            foreach (var obstacle in obstaclesPolys)
            {
                offset.AddPath(obstacle, JoinType.jtMiter, EndType.etClosedPolygon);
                offset.Execute(ref offsetContainer, ActorSize);

                foreach (var item in offsetContainer)
                {
                    yield return item;
                }
            }
        }

        private bool IsClockwisePolygon(IntPoint[] polygon)
        {
            bool isClockwise = false;
            double sum = 0;
            for (int i = 0; i < polygon.Length - 1; i++)
            {
                sum += (polygon[i + 1].X - polygon[i].X) * (polygon[i + 1].Y + polygon[i].Y);
            }
            isClockwise = (sum > 0) ? true : false;
            return isClockwise;
        }

        private class Edge :IEquatable<Edge>
        {
            public TriangulationPoint vertex1 { get; set; }
            public TriangulationPoint vertex2 { get; set; }

            public Edge(TriangulationPoint v1, TriangulationPoint v2)
            {
                vertex1 = v1;
                vertex2 = v2;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Edge);
            }

            public bool Equals(Edge edge)
            {
                return (edge.vertex1.Equals(vertex1) || edge.vertex1.Equals(vertex2)) && (edge.vertex2.Equals(vertex1) || edge.vertex2.Equals(vertex2));
            }

            public override int GetHashCode()
            {
                return vertex1.GetHashCode() ^ vertex2.GetHashCode();
            }

        }
    }
}
