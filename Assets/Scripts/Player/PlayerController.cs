using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets;
using Assets.Scripts;
using Assets.Scripts.Util;

/// <summary>
/// This class takes care of moving the player object and acting upon collisions
/// </summary>
public class PlayerController : NetworkBehaviour
{
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

    public int lastPlayerThatHit;
    public float MovementSpeed;

    public GameObject HpBarPrefab;
    private FollowPlayerHpBar hpBarFollowScript;

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
    }

    void OnLevelWasLoaded(int level)
    {
        navMesh = FindObjectOfType<NavMeshController>();
    }



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
            isMoving = true;
        }

        if (isOnLava)
        {
            StartHealth -= 0.1f;
        }
    }

    [ClientRpc]
    public void RpcDeath(int killerId)
    {
        StartHealth = 100;
        GameManager.Instance.PlayerKilled(killerId);
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

            int spellId = (int)spell.Type;
            RpcHit(collision.contacts[0].normal, spellId, spell.Level,spell.CasterID);
        }
    }

    private void HandlePlayerCollision(Vector2 collision, SpellCaster otherPlayer)
    {
        if (otherPlayer.IsDashing && isServer)
        {
            StartHealth -= 8;
            RpcHit(collision, (int)SpellType.Dash, 1,otherPlayer.playerId);
        }
    }

    /// <summary>
    /// Called by the server when someones hit
    /// </summary>
    /// <param name="collision"></param>
    [ClientRpc]
    void RpcHit(Vector2 collision, int spellIndex, int spellLvl, int casterId )
    {
        GameManager.Instance.addScore(casterId, 1);
        lastPlayerThatHit = casterId;
        isHit = true;
        collisionPoint = collision;
        spellHitWith = SpellManager.Instance[(SpellType)spellIndex];
        spellHitWith.SetSpellLevel(spellLvl);
    }

    void FixedUpdate()
    {
        if (isMoving)
            Move();

        if (isHit)
            KnockBack();
    }

    private void KnockBack()
    {
        rbody.AddForce(collisionPoint * (spellHitWith.BaseKnockback * KnockbackMultiplier));
        isHit = false;
    }

    private ISpell spellHitWith;
    private Vector3 NextPoint;
    private Vector2 collisionPoint;
    private bool isHit;
    private bool isMoving;
    private bool isOnLava;

    void Move()
    {
        SmoothPath();

        Vector3 relative = NextPoint - transform.position;
        Vector3 movementNormal = Vector3.Normalize(relative);
        var distance = Vector3.Distance(transform.position, NextPoint);

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
        float targetAngle = transform.AngleBetween(NextPoint) - 90;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public void StopMoving()
    {
        isMoving = false;
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

    public void SetupNavMesh()
    {
        navMesh = FindObjectOfType<NavMeshController>();
    }

    public void SetupHpBar()
    {
        GameObject hpPanel = Instantiate(HpBarPrefab) as GameObject;
        hpBarFollowScript = hpPanel.GetComponent<FollowPlayerHpBar>();
        //Make hpBar follow player
        hpBarFollowScript.player = gameObject;
        StartCoroutine(hpBarFollowScript.Fade());
    }

    void OnTriggerEnter2D()
    {
        isOnLava = false;
    }

    void OnTriggerExit2D()
    {
        isOnLava = true;
    }
}
