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
        public GameManager Gamemanager;

        void Awake()
        {
            Gamemanager = FindObjectOfType<GameManager>();
        }       

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            Gamemanager.StartManager();
            return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            Gamemanager.StartManager();
            base.OnLobbyClientSceneChanged(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            Gamemanager.ClearPlayers();
            base.OnClientDisconnect(conn);
        }

        public override void OnStopServer()
        {
            Gamemanager.ClearPlayers();
            base.OnStopServer();
        }
    }
}
