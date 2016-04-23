using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    class DashSpell : ISelfCast
    {
        public bool IsSelfCast
        {
            get
            {
                return true;
            }
        }

        private int _level = 1;
        public int Level
        {
            get
            {
                return _level;
            }
        }

        public SpellType Type
        {
            get
            {
                return SpellType.Dash;
            }
        }

        public int UpgradeCost
        {
            get
            {
                switch (_level)
                {
                    case 1:
                        return 7;

                    case 2:
                        return 9;

                    default:
                        return 0;
                }
            }
        }

        public float Damage
        {
            get
            {
                switch (_level)
                {
                    case 1:
                        return 7.5f;

                    case 2:
                        return 8.5f;

                    default:
                        return 10;
                }
            }
        }

        public float BaseKnockback
        {
            get
            {
                switch (_level)
                {
                    case 1:
                        return 600f;

                    case 2:
                        return 700f;

                    default:
                        return 10;
                }
            }
        }

        public float Cooldown { get; set; }

        public IEnumerator Execute(GameObject go)
        {
            var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            LeanTween.move(go, mousepos, Vector3.Distance(mousepos,go.transform.position) / 50);
            yield return null;
        }

        public void SetSpellLevel(int level)
        {
            _level = level;
        }

        public void UpgradeSpell()
        {
            _level++;

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
