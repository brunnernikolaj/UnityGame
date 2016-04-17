using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Assets;

namespace Assets.Scripts
{
    class CustomNetworkManager : NetworkLobbyManager
    {

        List<Player> _players = new List<Player>();

        public List<GameObject> SpawnPoints;

        public IEnumerable<Player> GetPlayers()
        {
            return _players;
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            

            return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
        }

        void Start()
        {
            var newTrans = new GameObject().transform;

            newTrans.position = new Vector3(20, 20, 0);
            startPositions.Add(newTrans);

        }
    }
}
