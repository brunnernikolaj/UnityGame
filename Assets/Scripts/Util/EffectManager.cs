using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Util
{
    class EffectManager : Singleton<EffectManager>
    {
        private GameObjectPool pool = new GameObjectPool("Prefab/Fireball Explosion");

        public  void SpawnEffect(EffectType type, Vector3 location,Quaternion rotatation)
        {
            var exp = pool.Spawn();
            exp.transform.position = location;
            exp.transform.rotation = rotatation;
            exp.GetComponent<ParticleSystem>().Play();

            StartCoroutine(pool.Despawn(exp, 2f));
        }
    }

    enum EffectType
    {
        FireballExplotion
    }
}
