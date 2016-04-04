using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;
using Assets.Scripts;
using System;

public class Bullet : NetworkBehaviour, IProjectile {

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (isServer)
        {
            Destroy(gameObject);
        }

    }



    // Use this for initialization
    void Start () {
       // GetComponent<Rigidbody2D>().velocity = transform.right * 30f;
    }
	
    public void lol()
    {
        
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void Fire()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * 30f;
    }
}
