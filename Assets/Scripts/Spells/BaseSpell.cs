using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Spells
{
    abstract class BaseSpell : ISpell
    {
        protected int _level;

        public abstract float BaseKnockback { get; }

        public int CasterID { get; set; }

        public float Cooldown { get; set; }

        public abstract float Damage { get; }

        public abstract string IconName { get; }

        public abstract bool IsSelfCast { get; }

        public int Level { get { return _level; } }

        public abstract SpellType Type { get; }

        public abstract int UpgradeCost { get; }

        public abstract void ResetCooldown();

        public void SetSpellLevel(int level)
        {
            _level = level;
        }

        public void UpgradeSpell()
        {
            _level++;
            ResetCooldown();
        }
    }
}
