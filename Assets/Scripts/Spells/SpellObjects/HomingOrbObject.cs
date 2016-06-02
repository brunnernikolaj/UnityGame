using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Spells.SpellObjects
{
    class HomingOrbObject : NetworkBehaviour , ISpellObject
    {
        public float TurnSpeed;
        public float Speed;

        private GameObject target;

        private ISpell spell = new HomingOrbSpell();
   

        public ISpell Spell
        {
            get
            {
                return spell;
            }
        }

        public void StartSpell()
        {
            
            Destroy(gameObject, 15f);
        }

        void Update()
        {
            transform.position = transform.position + transform.right * (Speed * Time.deltaTime);
            if (target == null)
            {
                int layerMask = 1 << 10;
                var hits = Physics2D.CircleCast(transform.position, 50f, Vector3.zero,0,layerMask);
                if (hits.collider != null && hits.collider.gameObject.GetComponent<SpellCaster>().playerId != spell.CasterID)
                {
                    target = hits.collider.gameObject;
                }
            }
            else
            {              

                Vector3 vectorToTarget = target.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * TurnSpeed);
            }
            
        }
        void OnDestroy()
        {
            EffectManager.Instance.SpawnEffect(EffectType.FireballExplotion, transform.position, transform.rotation);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (isServer)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }   
}
