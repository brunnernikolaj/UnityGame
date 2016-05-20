using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Spells
{
    public class FireballSpell : IProjectile
    {
        float ISpell.Damage
        {
            get
            {
                switch (_level)
                {
                    case 1:
                        return 10f;
                    case 2:
                        return 12f;

                    default:
                        return 10f;
                }
            }
        }

        public SpellType Type
        {
            get
            {
                return SpellType.Fireball;
            }
        }

        private int _level = 1;
        public int Level { get { return _level; } }

        float ISpell.BaseKnockback
        {
            get
            {
                switch (_level)
                {
                    case 1:
                        return 700f;
                    case 2:
                        return 750f;

                    default:
                        return 700f;
                }
            }
        }

        public int UpgradeCost
        {
            get
            {
                switch (_level)
                {
                    case 1:
                        return 10;
                    case 2:
                        return 12;

                    default:
                        return 0;
                }
            }
        }

        public bool IsSelfCast
        {
            get
            {
                return false;
            }
        }

        public int CasterID { get; set; }

        public float Cooldown { get; set; }

        public void UpgradeSpell()
        {
            _level++;
        }

        public void SetSpellLevel(int level)
        {
            _level = level;

            ResetCooldown();
        }

        public void ResetCooldown()
        {
            switch (_level)
            {
                case 1:
                    Cooldown = 4;
                    break;
                case 2:
                    Cooldown = 5;
                    break;
            }
        }
    }
}
