using Assets.Scripts.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
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
                Destroy(this.gameObject);


            DontDestroyOnLoad(this.gameObject);
        }

        public SpellManager()
        {
            Spells = new Dictionary<SpellType, ISpell>(new SpellComparer());
            Spells.Add(SpellType.Fireball, new FireballSpell());
            Spells.Add(SpellType.Dash, new DashSpell());
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

        internal GameObject GetSpell(int spellIndex)
        {
            return SpellPrefabs[0];
        }
    }

    public enum SpellType
    {
        Fireball,
        Dash
    };
}
