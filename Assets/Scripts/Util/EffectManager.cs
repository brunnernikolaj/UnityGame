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
        private Dictionary<EffectType, GameObjectPool> pools = new Dictionary<EffectType, GameObjectPool>();
        private GameObjectPool pool = new GameObjectPool("Prefab/Fireball Explosion");

        private EffectManager()
        {
            pools.Add(EffectType.FireballExplotion, new GameObjectPool("Prefab/Fireball Explosion"));
            pools.Add(EffectType.TeleportEffect, new GameObjectPool("Prefab/Teleport Effect"));
        }

        public  void SpawnEffect(EffectType type, Vector3 location,Quaternion rotatation)
        {
            var exp = pools[type].Spawn();
            exp.transform.position = location;
            exp.transform.rotation = rotatation;
            exp.GetComponent<ParticleSystem>().Play();

            StartCoroutine(pool.Despawn(exp, 2f));
        }

        void OnLevelWasLoaded(int level)
        {
            pool.Clear();
        }
    }

    enum EffectType
    {
        FireballExplotion,
        TeleportEffect
    }
}
