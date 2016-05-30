using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Assets.Scripts.Util;

/// <summary>
/// This class takes care of enabling scripts on the local player. and checking health
/// </summary>
public class PlayerNetworkManager : NetworkBehaviour
{
    private PlayerController player;
    private SpellCaster playerCaster;
    private int playerId;

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
        playerId = FindObjectOfType<GameManager>().getPlayerId();
        GetComponent<SpriteRenderer>().color = PlayerColors.getColor(playerId);
        player.SetupHpBar();

        GameManager.OnRoundStart += NewRound;

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

    private void NewRound(string sceneName)
    {
        transform.position = sceneName == "scene1" ?
            PlayerStartPositions.getPos(playerId) :
            PlayerStartPositions.getShopPos(playerId);
    }
}
