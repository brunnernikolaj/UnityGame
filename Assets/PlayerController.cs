using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed;

    private Stack<Vector2> path;
    private Rigidbody2D rbody;



    // Use this for initialization
    void Start()
    {
        path = new Stack<Vector2>();
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;

            path = new Stack<Vector2>(NavMeshController.Instance.FindPath(transform.position, mousepos));
            NextPoint = path.Pop();
            IsMoving = true;
            //LeanTween.move(gameObject, path.Pop(), 1f);
        }

    }

    void FixedUpdate()
    {
        if (IsMoving)
            Move();
    }

    private Vector3 NextPoint;
    private bool IsMoving;

    void Move()
    {
        int layerMask = 1 << 8;

        for (int i = 0; i < 5; i++)
        {
            if (path.Count > 0 && Physics2D.Linecast(transform.position, path.Peek(), layerMask).collider == null)
            {
                NextPoint = path.Pop();
            }
        }

        

        Vector3 relative = NextPoint - transform.position;
        Vector3 movementNormal = Vector3.Normalize(relative);
        var distance = Vector3.Distance(transform.position, NextPoint);
        float targetAngle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90;

        if (distance < 0.1)
        {
            if (path.Count == 0)
            {
                IsMoving = false;
                rbody.velocity = Vector3.zero;
                return;
            }

            NextPoint = path.Pop();
        }
        else
        {
            rbody.AddForce(new Vector2(movementNormal.x, movementNormal.y) * MovementSpeed);
        }

        transform.rotation = Quaternion.Euler(0, 0, targetAngle);

    }
}
