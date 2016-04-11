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

   

    public override void OnStartLocalPlayer()
    {

        GetComponent<PlayerController>().enabled = true;
    }
}
