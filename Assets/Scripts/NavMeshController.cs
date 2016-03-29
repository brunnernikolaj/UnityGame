using UnityEngine;
using System.Collections;
using Assets;
using System.Linq;
using System.Collections.Generic;
using Poly2Tri;
using System.Diagnostics;
using System;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class NavMeshController : MonoBehaviour
{

    public int ObstacleLayer { get; set; }
    public int WalkableLayer { get; set; }

    public bool ShouldDrawConnections { get; set; }

    NavMeshGenerator navMeshGenerator = new NavMeshGenerator();
    NavMesh2D navMesh;

    /// <summary>
    /// Find walkable and obstacle points, then build NavMesh
    /// </summary>
    public void BuildMesh()
    {
        var walkablePoly = FindCollidorPoints(go => go.layer == WalkableLayer).SelectMany(x => x);
        var obstaclesPolys = FindCollidorPoints(go => go.layer == ObstacleLayer).Select(x => x);

        navMesh = navMeshGenerator.Generate(walkablePoly, obstaclesPolys);

        GetComponent<MeshFilter>().mesh = navMeshGenerator.Mesh;
    }

    /// <summary>
    /// Builds a list of lists containing world cordinates of collidor points
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    private IEnumerable<IEnumerable<Vector3>> FindCollidorPoints(Func<GameObject, bool> predicate)
    {
        foreach (var go in FindObjectsOfType<GameObject>())
            if (predicate(go))
                yield return go.GetComponent<PolygonCollider2D>().points.Select(point => Vector3.Scale(go.transform.TransformPoint(point), new Vector2(10,10)));
    }

    public GameObject Location;
    public GameObject Target;
    List<Vector2> path;

    /// <summary>
    /// Find a Path from location to target using NavMesh2D
    /// </summary>
    public void FindPath()
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();
        path = navMesh.PathFind(Location.transform.position, Target.transform.position);
        sw.Stop();

        UnityEngine.Debug.Log("Found path in " + sw.Elapsed.Milliseconds + "ms");
    }


    void OnDrawGizmos()
    {
        DrawTestPath();

        if (ShouldDrawConnections)
            DrawConnctions();
    }

    /// <summary>
    /// Draws lines between neighbours in the navMesh
    /// </summary>
    private void DrawConnctions()
    {
        if (navMeshGenerator.Mesh != null)
        {
            Gizmos.color = Color.white;

            foreach (var triangle in navMesh.Triangles)
            {
                var center = triangle.Centroid();
                var pos = new Vector3((float)center.X, (float)center.Y, 0);

                foreach (var neighbor in triangle.Neighbors.Where(n => n != null))
                {
                    var center2 = neighbor.Centroid();
                    var pos2 = new Vector3((float)center2.X, (float)center2.Y, 0);
                    Gizmos.DrawLine(pos, pos2);
                }
            }
        }
    }

    /// <summary>
    /// Draws a line from location to target using the path from NavMesh2D 
    /// </summary>
    private void DrawTestPath()
    {
        if (path != null)
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < path.Count - 1; i++)
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}
