using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkManager : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnStartClient()
    {
        GetComponent<PlayerController>().SetupHpBar();
        base.OnStartClient();
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<PlayerController>().enabled = true;
        GetComponent<SpellCaster>().enabled = true;
    }
}
