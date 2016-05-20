using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Assets.Scripts.Util;

public class PlayerNetworkManager : NetworkBehaviour
{
    private PlayerController player;
    private SpellCaster playerCaster;

    // Use this for initialization
    void Start()
    {
        //player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.StartHealth <= 0 && isServer)
        {
            player.RpcDeath(player.lastPlayerThatHit);
        }
    }

    public override void OnStartClient()
    {
        player = GetComponent<PlayerController>();
        playerCaster = GetComponent<SpellCaster>();
        GetComponent<SpriteRenderer>().color = PlayerColors.getColor(playerCaster.playerId);
        player.SetupHpBar();
        base.OnStartClient();
    }

    void OnLevelWasLoaded(int level)
    {
        GetComponent<PlayerController>().SetupHpBar();
    }

    public override void OnStartLocalPlayer()
    {

        player.enabled = true;
        player.SetupNavMesh();
        playerCaster.enabled = true;
    }
}
