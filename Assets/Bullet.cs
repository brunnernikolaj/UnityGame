using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;
using Assets.Scripts;
using System;
using Assets.Scripts.Spells;

public class Bullet : NetworkBehaviour , ISpellObject{

    private FireballSpell spell = new FireballSpell();
    public ISpell Spell
    {
        get
        {
            return spell;
        }
    }

    public void StartSpell()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * 30f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (isServer)
        {
            Destroy(gameObject);
        }

    }



    // Use this for initialization
    void Start () {
        
    }
	

	// Update is called once per frame
	void Update () {
	
	}


}
