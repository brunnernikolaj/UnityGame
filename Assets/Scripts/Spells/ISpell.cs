using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public interface ISpell
    {
        float Damage { get; }

        float KnockbackMultiplier { get; }

        float BaseKnockback { get; }

        SpellType Type { get; }
    }
}
