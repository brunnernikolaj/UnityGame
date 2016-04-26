using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Util
{
    class GameObjectPool : NetworkBehaviour
    {
        private Stack<GameObject> pool = new Stack<GameObject>();

        private GameObject Prefab;

        public GameObjectPool(string PrefabLocation)
        {
            Prefab = Resources.Load<GameObject>(PrefabLocation);
        }

        public void Clear()
        {
            pool.Clear();
        }

        public GameObject Spawn()
        {
            GameObject go = null;

            if (pool.Count <= 0)
                go = Instantiate(Prefab);
            else
                go = pool.Pop();

            go.SetActive(true);

            return go;
        }

        public IEnumerator Despawn(GameObject spawned,float time)
        {
            yield return new WaitForSeconds(time);
            if (spawned != null)
            {
                spawned.SetActive(false);
                pool.Push(spawned);
            }
           
        }
    }
}
