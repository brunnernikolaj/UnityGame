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

        private static CustomNetworkManager _instance;

        private bool isManagerStarted = false;

        void Awake()
        {
            //if we don't have an [_instance] set yet
            if (!_instance)
                _instance = this;
            //otherwise, if we do, kill this thing
            else
                Destroy(this.gameObject);

        }

        void Start()
        {
            //if (FindObjectsOfType<CustomNetworkManager>() != null)
            //{
            //    Destroy(gameObject);
            //}
        }


        //public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        //{
        //    if (!isManagerStarted)
        //    {
        //      Gamemanager.StartManager();
        //        isManagerStarted = true;
        //    }
            
        //    return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
        //}


        public override void OnStartClient(NetworkClient lobbyClient)
        {
            Gamemanager.StartManager();
            base.OnStartClient(lobbyClient);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            NetworkServer.SetAllClientsNotReady();
            Gamemanager.ClearPlayers();
            base.OnClientDisconnect(conn);
            Destroy(gameObject);
        }

        //public override void OnStopServer()
        //{
        //    //NetworkServer.Reset();
        //    Gamemanager.ClearPlayers();
        //    base.OnStopServer();
        //}
    }
}
