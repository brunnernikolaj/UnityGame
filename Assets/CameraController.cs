using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public int Boundary; // distance from edge scrolling starts
    public int speed;
    private int theScreenWidth;
    private int theScreenHeight;

    // Use this for initialization
    void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.)
        //{


        //}

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
}
