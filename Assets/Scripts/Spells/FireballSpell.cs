using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Spells
{
    public class FireballSpell : IProjectile
    {
        public float Damage
        {
            get
            {
                return 10.0f;
            }
        }

        public float KnockbackMultiplier
        {
            get
            {
                return 0.1f;
            }
        }

        public float BaseKnockback
        {
            get
            {
                return 700f;
            }
        }

        public SpellType Type
        {
            get
            {
                return SpellType.Fireball;
            }
        }

        public void Fire()
        {
            throw new NotImplementedException();
        }
    }
}
