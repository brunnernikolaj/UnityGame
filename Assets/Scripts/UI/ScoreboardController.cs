using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets.Scripts.Util;

public class ScoreboardController : Singleton<ScoreboardController> {

    public GameObject scoreEntryPrefab;
    public GameObject Container;

    private List<GameObject> _scores = new List<GameObject>();

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}

    public GameObject AddPlayer(Player player)
    {       
        var go = Instantiate(scoreEntryPrefab);
        go.transform.SetParent(Container.transform);
        var gos = go.GetComponentsInChildren<Text>();

        gos[0].text = player.Name;
        gos[1].text = "0";

        go.GetComponentsInChildren<Image>()[1].color = PlayerColors.getColor(player.Id);

        NetworkServer.Spawn(go);
        _scores.Add(go);

        return go;
    }

    public void UpdateScore(int playerId, int score)
    {
        var gos = _scores[playerId].GetComponentsInChildren<Text>();

        gos[1].text = score.ToString();
    }

    internal void Clear()
    {
        foreach (var item in _scores)
        {
            Destroy(item);
        }
    }
}
