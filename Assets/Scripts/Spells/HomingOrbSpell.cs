using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Spells
{
    class HomingOrbSpell : BaseSpell
    {

        public override float BaseKnockback
        {
            get
            {
                switch (_level)
                {
                    case 0:
                        return 600;
                    case 1:
                        return 700;
                    case 2:
                        return 800;
                    case 3:
                        return 900;

                    default:
                        return 0;
                }
            }
        }


        public override float Damage
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

        public override string IconName
        {
            get
            {
                return "HomingOrb";
            }
        }

        public override bool IsSelfCast
        {
            get
            {
                return false;
            }
        }


        public override SpellType Type
        {
            get
            {
                return SpellType.HomingOrb;
            }
        }

        public override int UpgradeCost
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
       

        public override void ResetCooldown()
        {
            switch (_level)
            {
                case 1:
                    Cooldown = 4;
                    break;
                case 2:
                    Cooldown = 3;
                    break;
            }
        }
    }
}
