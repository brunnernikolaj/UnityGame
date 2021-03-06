﻿using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    class TeleportSpell : ISelfCast
    {
        public bool IsSelfCast
        {
            get
            {
                return true;
            }
        }

        private int _level;
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
                throw new NotImplementedException();
            }
        }

        public int UpgradeCost
        {
            get
            {
                switch (_level)
                {
                    case 0:
                        return 6;
                    case 1:
                        return 7;
                    case 2:
                        return 8;
                    case 3:
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
                throw new NotImplementedException();
            }
        }

        public float BaseKnockback
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float Cooldown { get; set; }

        IEnumerator ISelfCast.Execute(GameObject go)
        {
            EffectManager.Instance.SpawnEffect(EffectType.TeleportEffect, go.transform.position, go.transform.rotation);
            yield return new WaitForSeconds(0.2f);
            var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            go.transform.position = mousepos;
            EffectManager.Instance.SpawnEffect(EffectType.TeleportEffect, mousepos, go.transform.rotation);
        }

        public void SetSpellLevel(int level)
        {
            _level = level;
        }

        public int CasterID { get; set; }

        public string IconName
        {
            get
            {
                return "Teleport";
            }
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
