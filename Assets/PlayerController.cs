using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets;
using Assets.Scripts;

public class PlayerController : NetworkBehaviour
{
    public GameObject BulletPrefab;
    public GameObject SpawnPoint;
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
        }

        if (Input.GetMouseButtonDown(0))
        {
            var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            CmdFire(mousepos);
        }
    }

    [Command]
    private void CmdFire(Vector3 mousepos)
    {
        float targetAngle = transform.AngleBetween(mousepos);

        transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);

        var bullet = (GameObject)Instantiate(BulletPrefab, SpawnPoint.transform.position, Quaternion.Euler(0, 0, targetAngle));
        bullet.GetComponent<IProjectile>().Fire();

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 5);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isServer)
            RpcHit(collision.contacts[0].normal);     
    }

    [ClientRpc]
    void RpcHit(Vector2 collision)
    {
        isHit = true;
        col = collision;
    }

    bool isHit;
    Vector2 col;

    void FixedUpdate()
    {
        if (IsMoving)
            RpcMove();

        if (isHit)
        {
            var ridgedBody = GetComponent<Rigidbody2D>();
            ridgedBody.AddForce(col * 900f);
            //transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(ridgedBody.velocity.x, ridgedBody.velocity.y, 0), Time.deltaTime * 5);
            isHit = false;
        }
    }

    private Vector3 NextPoint;
    private bool IsMoving;

    void RpcMove()
    {
        int layerMask = 1 << 8;

        for (int i = 0; i < 3; i++)
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
