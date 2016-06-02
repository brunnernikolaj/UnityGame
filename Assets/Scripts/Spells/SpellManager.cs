using Assets.Scripts.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This class is used to find spells from a enum when sent over the network.
    /// This class also contains spell prefabs
    /// </summary>
    class SpellManager : MonoBehaviour
    {
        private Dictionary<SpellType, ISpell> Spells;

        public List<GameObject> SpellPrefabs;

        public ISpell this[SpellType type]
        {
            get { return Spells[type]; }
        }

        private static SpellManager _instance;
        public static SpellManager Instance { get { return _instance; } }

        void Awake()
        {
            //if we don't have an [_instance] set yet
            if (!_instance)
                _instance = this;
            //otherwise, if we do, kill this thing
            else
                Destroy(gameObject);


            DontDestroyOnLoad(gameObject);
        }

        public SpellManager()
        {
            Spells = new Dictionary<SpellType, ISpell>(new SpellComparer());
            Spells.Add(SpellType.Fireball, new FireballSpell());
            Spells.Add(SpellType.Dash, new DashSpell());
            Spells.Add(SpellType.HomingOrb, new HomingOrbSpell());
        }        

        internal class SpellComparer : IEqualityComparer<SpellType>
        {
            public bool Equals(SpellType x, SpellType y)
            {
                return x == y;
            }

            public int GetHashCode(SpellType obj)
            {
                return obj.GetHashCode();
            }
        }

        public GameObject GetSpell(int spellIndex)
        {
            return SpellPrefabs[spellIndex];
        }
    }

    public enum SpellType
    {
        Fireball,
        HomingOrb,
        Dash
    };
}
