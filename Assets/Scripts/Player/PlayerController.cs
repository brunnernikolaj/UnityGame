using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets;
using Assets.Scripts;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public float MovementSpeed;

    [SyncVar(hook = "OnStartHealthChange")]
    public float StartHealth;
    private void OnStartHealthChange(float updatedHealth)
    {
        StartHealth = updatedHealth;
        hpBarFollowScript.SetHealth(StartHealth);
        hpBarFollowScript.Show();
    }

    [SyncVar]
    public float KnockbackMultiplier;

    public GameObject HpBarPrefab;
    private GameObject hpPanel;

    private Stack<Vector2> path;
    private Rigidbody2D rbody;

    private NavMeshController navMesh;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            //var canvas = FindObjectOfType<Canvas>();
            //HpText = canvas.transform.Find("Text").GetComponent<Text>();

            //HpText.text = "Current Hp: " + StartHealth;
        }
        path = new Stack<Vector2>();
        rbody = GetComponent<Rigidbody2D>();

        GameManager.OnRoundStart += NewRound;       
    }

    private FollowPlayerHpBar hpBarFollowScript;

    public void SetupHpBar()
    {
        hpPanel = Instantiate(HpBarPrefab) as GameObject;
        hpBarFollowScript = hpPanel.GetComponent<FollowPlayerHpBar>();
        hpBarFollowScript.player = gameObject;
        StartCoroutine(hpBarFollowScript.Fade());
    }

    void OnLevelWasLoaded(int level)
    {
        navMesh = FindObjectOfType<NavMeshController>();
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

            var tempPath = navMesh.FindPath(transform.position, mousepos);

            //No path found
            if (tempPath == null)
                return;

            path = new Stack<Vector2>(tempPath);

            NextPoint = path.Pop();
            IsMoving = true;
        }


    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 collisionNormal = collision.contacts[0].normal;

        if (collision.gameObject.tag == "Player")
        {
            HandlePlayerCollision(collisionNormal, collision.gameObject.GetComponent<SpellCaster>());
        }
        else if (collision.gameObject.tag == "Spell")
        {
            var spell = collision.gameObject.GetComponent<ISpellObject>().Spell as IProjectile;
            HandleSpellCollision(collision, spell);
        }

    }

    private void HandleSpellCollision(Collision2D collision, IProjectile spell)
    {
        if (isServer)
        {


            StartHealth -= spell.Damage;
            KnockbackMultiplier += 0.10f;

            if (StartHealth <= 0)
            {
                //GameManager.Instance.PlayerKilled((int)netId.Value,0);
            }

            int spellId = (int)spell.Type;
            RpcHit(collision.contacts[0].normal, spellId, spell.Level);
        }
    }

    private void HandlePlayerCollision(Vector2 collision, SpellCaster otherPlayer)
    {
        if (otherPlayer.IsDashing && isClient)
        {
            StartHealth -= 8;
            RpcHit(collision, (int)SpellType.Dash, 1);
        }
    }


    /// <summary>
    /// Called by the server when someones hit
    /// </summary>
    /// <param name="collision"></param>
    [ClientRpc]
    void RpcHit(Vector2 collision, int spellIndex, int spellLvl)
    {

        isHit = true;
        col = collision;
        spellHitWith = SpellManager.Instance[(SpellType)spellIndex];
        spellHitWith.SetSpellLevel(spellLvl);
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
        hpBarFollowScript.Show();
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

        if (distance < 0.2)
        {
            if (path.Count == 0)
            {
                StopMoving();
                return;
            }

            NextPoint = path.Pop();

        }
        else
        {
            rbody.AddForce(new Vector2(movementNormal.x, movementNormal.y) * MovementSpeed, ForceMode2D.Force);
        }
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public void StopMoving()
    {
        IsMoving = false;
        rbody.velocity = Vector3.zero;
    }

    private void SmoothPath()
    {
        int layerMask = 1 << 8;

        for (int i = 0; i < 3; i++)
        {
            if (path.Count <= 0)
            {
                break;
            }

            var nextPoint = path.Peek();
            Vector3 relative = new Vector3(nextPoint.x, nextPoint.y) - transform.position;
            Vector3 movementNormal = Vector3.Normalize(relative);
            var direction = new Vector2(movementNormal.x, movementNormal.y);
            var col = Physics2D.CircleCast(transform.position, 1f, direction, Vector3.Distance(transform.position, nextPoint), layerMask);
            if (col.transform == null)
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
