using UnityEngine;
using System.Collections;

/// <summary>
/// This class makes sure the camera stays inside the playing area
/// </summary>
public class CameraController : MonoBehaviour
{
    public int Boundary; // distance from edge scrolling starts
    public int speed;
    private int theScreenWidth;
    private int theScreenHeight;

    public float mapX;
    public float mapY;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    // Use this for initialization
    void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;

        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = (vertExtent * Screen.width / Screen.height);

        minX = horzExtent;
        maxX = mapX - minX ;
        minY = vertExtent;
        maxY = mapY - horzExtent / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x > theScreenWidth - Boundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + speed, transform.position.y,-10) , Time.deltaTime); // move on +X axis
        }
        if (Input.mousePosition.x < 0 + Boundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - speed, transform.position.y, -10), Time.deltaTime); // move on -X axis
        }
        if (Input.mousePosition.y > theScreenHeight - Boundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + speed, -10), Time.deltaTime); // move on +Z axis
        }
        if (Input.mousePosition.y < 0 + Boundary)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - speed, -10), Time.deltaTime); // move on -Z axis
        }
    }

    void LateUpdate()
    {
        var v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        transform.position = v3;
    }
}
