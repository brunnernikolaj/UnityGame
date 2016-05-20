using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class Player
    {
        public int Id { get; set; }

        public int Score { get; set; }

        public string Name { get; set; }

        public int Gold { get; set; }

        public Dictionary<KeyCode, ISpell> Spells { get; set; }
    }
}
