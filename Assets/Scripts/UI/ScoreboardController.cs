using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ScoreboardController : Singleton<ScoreboardController> {

    public GameObject scoreEntryPrefab;

    private List<GameObject> _scores = new List<GameObject>();

	// Use this for initialization
	void Start () {

	}

    public GameObject AddPlayer(Player player)
    {       
        var go = Instantiate(scoreEntryPrefab);
        go.transform.SetParent(this.transform);
        var gos = go.GetComponentsInChildren<Text>();

        gos[0].text = player.Name;

        _scores.Add(go);

        return go;
    }

    internal void Clear()
    {
        foreach (var item in _scores)
        {
            Destroy(item);
        }
    }
}
