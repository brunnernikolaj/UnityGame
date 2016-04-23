using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public interface ISpellObject
    {
        ISpell Spell { get; }

        /// <summary>
        /// Called Before network spawn to setup the spell
        /// </summary>
        void StartSpell();
    }
}
