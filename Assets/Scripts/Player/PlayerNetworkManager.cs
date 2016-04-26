using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkManager : NetworkBehaviour
{
    private PlayerController player;

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
            player.RpcDeath();
        }
    }

    public override void OnStartClient()
    {
        player = GetComponent<PlayerController>();
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
        GetComponent<SpellCaster>().enabled = true;
    }
}
