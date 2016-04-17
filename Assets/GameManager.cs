using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets;
using Assets.Scripts;

public class GameManager : NetworkBehaviour{

    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

        public int Rounds;

    public delegate void RoundStart();

    List<Player> _players = new List<Player>();

    public IEnumerable<Player> GetPlayers()
    {
        return _players;
    }

    public static event RoundStart OnRoundStart;


    private int playerCount;

	// Use this for initialization
	void Start () {
        var players = (NetworkLobbyManager.singleton as NetworkLobbyManager).lobbySlots;

        foreach (var item in players)
        {
            if (item != null)
            {
                ScoreboardController.Instance.AddPlayer(new Player { Name = "Lars" });
            }
           
        }
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

    public void PlayerKilled(int killed, int killer)
    {
        _players[killer].Score++;

        if (playerCount <= 1)
        {
            EndRound();
        }
        else
        {
            playerCount--;
        }
    }

    public void EndRound()
    {
        Rounds--;
        if (Rounds == 0)
        {
            EndGame();
        }

        OnRoundStart();
    }

    private void EndGame()
    {
        throw new NotImplementedException();

    }
}
