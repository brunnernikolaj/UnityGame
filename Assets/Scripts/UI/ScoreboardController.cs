using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets.Scripts.Util;

public class ScoreboardController : MonoBehaviour {

    public GameObject scoreEntryPrefab;
    public GameObject Container;

    private List<GameObject> _scores = new List<GameObject>();

    private static ScoreboardController _instance;
    public static ScoreboardController Instance { get { return _instance; } }

    // Use this for initialization
    void Awake () {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
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

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
            Destroy(gameObject);

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
