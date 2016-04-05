using Assets.Scripts.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class SpellManager : Singleton<SpellManager>
    {
        private Dictionary<SpellType, ISpell> Spells;

        public ISpell this[SpellType type]
        {
            get { return Spells[type]; }
        }

        public SpellManager()
        {
            Spells = new Dictionary<SpellType, ISpell>(new SpellComparer());
            Spells.Add(SpellType.Fireball, new FireballSpell());
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

       
    }

    public enum SpellType
    {
        Fireball
    };
}
