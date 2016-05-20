using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Assets;
using Assets.Scripts;
using System.Collections.Generic;
using Assets.Scripts.Spells;

public class SpellCaster : NetworkBehaviour {

    public GameObject BulletPrefab;
    public GameObject SpawnPoint;

    private ISpell selectedSpell;
    private KeyCode selectedKey;
    public bool IsDashing { get; private set; }

    private Player localPlayer;
    public int playerId;

    private SpellManager spellManager;

    private PlayerController playerCtrl;

    // Use this for initialization
    void Start () {
        playerCtrl = GetComponent<PlayerController>();
        localPlayer = FindObjectOfType<GameManager>().GetLocalPlayer();
        playerId = localPlayer.Id;
        spellManager = FindObjectOfType<SpellManager>();
	}

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {


        foreach (var cooldown in localPlayer.Spells)
        {
            if (cooldown.Value.Cooldown >= 0)
            {
                cooldown.Value.Cooldown = cooldown.Value.Cooldown - Time.deltaTime;
            }
        }

        foreach (var key in localPlayer.Spells.Keys)
        {
            if (Input.GetKeyDown(key) && localPlayer.Spells[key].Cooldown <= 0)
            {
                selectedKey = key;
                selectedSpell = localPlayer.Spells[key];
                selectedSpell.CasterID = localPlayer.Id;
            }
        }

        if (Input.GetMouseButtonDown(0) && selectedSpell != null)
        {
            var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            if (selectedSpell is ISelfCast)
            {
                if (selectedSpell is DashSpell)
                {
                    IsDashing = true;
                }

                playerCtrl.StopMoving();
                StartCoroutine((selectedSpell as ISelfCast).Execute(gameObject));

                
            }
            else
            {
                CmdFire(mousepos, (int)selectedSpell.Type, selectedSpell.Level);
            }
            localPlayer.Spells[selectedKey].ResetCooldown();
            selectedSpell = null;
        }    
        
    }

    [Command]
    private void CmdFire(Vector3 mousepos, int spellIndex, int spellLevel)
    {
        float targetAngle = transform.AngleBetween(mousepos);

        transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);

        var prefab = SpellManager.Instance.GetSpell(spellIndex);

        var spell = (GameObject)Instantiate(prefab, SpawnPoint.transform.position, Quaternion.Euler(0, 0, targetAngle));
        var spellObject = spell.GetComponent<ISpellObject>();
        spellObject.StartSpell();
        spellObject.Spell.SetSpellLevel(spellLevel);
        NetworkServer.Spawn(spell);
    }
}
