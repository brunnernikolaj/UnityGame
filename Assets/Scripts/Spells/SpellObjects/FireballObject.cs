using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;
using Assets.Scripts;
using System;
using Assets.Scripts.Spells;
using Assets.Scripts.Util;

public class FireballObject : NetworkBehaviour , ISpellObject{

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
        Destroy(gameObject, 3f);
    }

    void OnDestroy()
    {
        EffectManager.Instance.SpawnEffect(EffectType.FireballExplotion, transform.position, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isServer)
        {
            Destroy(gameObject);
        }
    }
}
