using UnityEngine;
using System.Collections;
using System.Linq;

public class HexagonShape : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        float half = transform.lossyScale.x / 2;
        float quart = transform.lossyScale.y / 4;

        Mesh msh = new Mesh();

        var points = new Vector3[]
        {
            new Vector3(10 + quart, 10),
            new Vector3(10 - quart, 10),
            new Vector3(10, 10 + half),
            new Vector3(10 - quart, -10),
            new Vector3(10 + quart, -10),
            new Vector3(10, 10 + half)

        };

        var triangles = new int[]
            {
            3,1,0,
            5,4,3
        };


        msh.vertices = points;
        msh.colors = points.Select(x => Color.blue).ToArray();
        msh.triangles = triangles;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = msh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
