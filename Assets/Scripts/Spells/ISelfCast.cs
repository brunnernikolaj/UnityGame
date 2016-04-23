using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    interface ISelfCast : ISpell
    {
        IEnumerator Execute(GameObject go);
    }
}
