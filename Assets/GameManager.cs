using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets;

public class GameManager : Singleton<GameManager> {

    public int Rounds;

    public delegate void RoundStart();

    public IEnumerable<Player> GetPlayers()
    {
        return _players.Values;
    }

    public static event RoundStart OnRoundStart;

    private Dictionary<int,Player> _players = new Dictionary<int, Player>();
    private int playerCount;

	// Use this for initialization
	void Awake () {
        _players.Add(3, new Player { Name ="abe"});
    }

    public void AddPlayer(int id, Player player)
    {
        _players.Add(id, player);
        playerCount++;
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
