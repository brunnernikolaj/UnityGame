using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ISpell
    {
        int CasterID { get; set; }

        float Damage { get; }

        float BaseKnockback { get; }

        float Cooldown { get; set; }

        void ResetCooldown();

        int Level { get; }

        int UpgradeCost { get; }

        SpellType Type { get; }

        bool IsSelfCast { get; }
        string IconName { get; }

        void UpgradeSpell();

        void SetSpellLevel(int level);
    }
}
