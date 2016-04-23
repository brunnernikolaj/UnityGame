using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class BaseSpell : NetworkBehaviour
    {
        public float Damage;
        public float DamagePerLvL;

        public float BaseKnockback;
    }
}
