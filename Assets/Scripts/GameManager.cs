using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets;
using Assets.Scripts;
using System.Linq;
using Assets.Scripts.Spells;

public class GameManager : NetworkBehaviour{

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public int Rounds;

    public delegate void RoundStart();

    List<Player> _players = new List<Player>();

    public void ClearPlayers()
    {
        _players = new List<Player>();
    }

    public IEnumerable<Player> GetPlayers()
    {
        return _players;
    }

    public Player GetLocalPlayer()
    {
        if (_players.Count == 0)
        {
            StartManager();
        }

        return localPlayer;
    }

    public static event RoundStart OnRoundStart;

    private Player localPlayer;

    private int playerCount;

    private CustomNetworkManager netManager;
    private NavMeshController navController;

    void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);        
    }

    // Use this for initialization
    void Start () {
        netManager = FindObjectOfType<CustomNetworkManager>();
        navController = FindObjectOfType<NavMeshController>();
    }

    public void StartManager()
    {
        var players = (NetworkLobbyManager.singleton as NetworkLobbyManager).lobbySlots;

        foreach (var item in players)
        {
            if (item != null)
            {
                var player = new Player { Name = "Lars", Gold = 50};
                player.Spells = new Dictionary<KeyCode, ISpell>();
                player.Spells.Add(KeyCode.A, new FireballSpell());
                if (item.isLocalPlayer)
                {
                    localPlayer = player;
                }
                if (isServer && item.isServer)
                {
                    localPlayer = player;
                }

                _players.Add(player);
                //ScoreboardController.Instance.AddPlayer(player);
            }

        }

        playerCount = _players.Count;
    }

    public void AddPlayer( Player player)
    {
        CmdPlayerJoined();
    }

    [Command]
    void CmdPlayerJoined()
    {
        //var player = new Player { Name = "Lars" };
        //_players.Add(player);

        //ScoreboardController.Instance.Clear();

        //foreach (var item in _players)
        //{
        //    NetworkServer.Spawn();
        //}
    }

    public void ChangeScene(string sceneName)
    {
        netManager.ServerChangeScene(sceneName);
    }

    public void PlayerKilled(int killed, int killer)
    {
        //_players[killer].Score++;
        playerCount--;
        if (playerCount <= 1)
        {
            EndRound();
        }
    }

    public void EndRound()
    {
        Rounds--;
        if (Rounds == 0)
        {
            EndGame();
        }
        else
        {
            playerCount = _players.Count;
            ChangeScene("ShopScene");
        }

        OnRoundStart();
    }

    private void EndGame()
    {
        throw new NotImplementedException();

    }
}
