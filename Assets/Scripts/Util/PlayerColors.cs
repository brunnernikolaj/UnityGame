using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// This class is used to color the players
    /// </summary>
    class PlayerColors
    {
        public static Color getColor(int id)
        {
            switch (id)
            {
                case 0:
                    return Color.blue;
                case 1:
                    return Color.red;
                case 2:
                    return Color.green;
                case 3:
                    return Color.yellow;
            }
            return Color.white;
        }
    }
}
