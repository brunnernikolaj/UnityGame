using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Util
{
    class PlayerStartPositions
    {
        public static Vector3 getPos(int pos)
        {
            switch (pos)
            {
                case 0:
                    return new Vector3(115, 115, 0);
                case 1:
                    return new Vector3(115, 285, 0);
                case 2:
                    return new Vector3(285, 285, 0);
                case 3:
                    return new Vector3(285, 115, 0);
            }
            return Vector3.one;
        }

        public static Vector3 getShopPos(int pos)
        {
            switch (pos)
            {
                case 0:
                    return new Vector3(100, 150, 0);
                case 1:
                    return new Vector3(150, 150, 0);
                case 2:
                    return new Vector3(200, 150, 0);
                case 3:
                    return new Vector3(250, 150, 0);
            }
            return Vector3.one;
        }
    }
}
