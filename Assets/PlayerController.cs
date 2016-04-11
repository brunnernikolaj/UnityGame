using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets;
using Assets.Scripts;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public GameObject BulletPrefab;
    public GameObject SpawnPoint;

    public float MovementSpeed;

    [SyncVar(hook ="OnStartHealthChange")]
    public float StartHealth;
    private void OnStartHealthChange(float updatedHealth)
    {
        StartHealth = updatedHealth;
        HpText.text = "Current Hp: " + StartHealth;
    }

    [SyncVar]
    public float KnockbackMultiplier;

    private Text HpText;
    private Stack<Vector2> path;
    private Rigidbody2D rbody;



    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            var canvas = FindObjectOfType<Canvas>();
            HpText = canvas.transform.Find("Text").GetComponent<Text>();

            HpText.text = "Current Hp: " + StartHealth;
        }
        path = new Stack<Vector2>();
        rbody = GetComponent<Rigidbody2D>();

        GameManager.OnRoundStart += NewRound;
    }

    private void NewRound()
    {
        transform.position = new Vector3(Random.Range(100, 200), Random.Range(100, 200));
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;

            var tempPath = NavMeshController.Instance.FindPath(transform.position, mousepos);

            //No path found
            if (tempPath == null)
                return;

            path = new Stack<Vector2>(tempPath);

            NextPoint = path.Pop();
            IsMoving = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.PlayerKilled((int)netId.Value,0);
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
        bullet.GetComponent<ISpellObject>().StartSpell();
        NetworkServer.Spawn(bullet);

        Destroy(bullet, 5);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var spell = collision.gameObject.GetComponent<ISpellObject>().Spell;
        if (spell != null)
        {
            if (spell is IProjectile)
            {
                Debug.Log("Nuce");

            }

            if (isServer)
            {
                StartHealth -= spell.Damage;
                KnockbackMultiplier += spell.KnockbackMultiplier;

                if (StartHealth <= 0)
                {
                    GameManager.Instance.PlayerKilled((int)netId.Value,0);
                }

                int spellId = (int)spell.Type;
                RpcHit(collision.contacts[0].normal, spellId);               
            }
               
        }

         
    }
    /// <summary>
    /// Called by the server when someones hit
    /// </summary>
    /// <param name="collision"></param>
    [ClientRpc]
    void RpcHit(Vector2 collision, int spellIndex)
    {
        isHit = true;
        col = collision;
        spellHitWith = SpellManager.Instance[(SpellType)spellIndex];
    }

    ISpell spellHitWith;
    bool isHit;
    Vector2 col;

    void FixedUpdate()
    {
        if (IsMoving)
            Move();

        if (isHit)
            KnockBack();
    }

    private void KnockBack()
    {
        var ridgedBody = GetComponent<Rigidbody2D>();
        ridgedBody.AddForce(col * (spellHitWith.BaseKnockback * KnockbackMultiplier));
        isHit = false;
    }

    private Vector3 NextPoint;
    private bool IsMoving;

    void Move()
    {
        SmoothPath();
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

    private void SmoothPath()
    {
        int layerMask = 1 << 8;

        for (int i = 0; i < 3; i++)
        {
            if (path.Count > 0 && Physics2D.Linecast(transform.position, path.Peek(), layerMask).collider == null)
            {
                NextPoint = path.Pop();
            }
            else
            {
                break;
            }
        }
    }
}
