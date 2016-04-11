using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Assets;

namespace Assets.Scripts
{
    class CustomNetworkManager : NetworkManager
    {
        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
            Debug.Log(conn.connectionId + " connected");

            GameManager.Instance.AddPlayer(conn.connectionId, new Player {Name = "Lars" });
        }
    }
}
