﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class represents a timer in the shop scene. That will change scene to the main scene
/// </summary>
public class Timer : NetworkBehaviour {

    public float CountdownTime;
    public Text TimeRemaingText;

    private GameManager manager;

	// Use this for initialization
	void Start () {
        manager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        CountdownTime -= Time.deltaTime;

        TimeRemaingText.text = Mathf.RoundToInt(CountdownTime).ToString();

        if (CountdownTime <= 0)
        {
            manager.ChangeScene("scene1");
        }
	}    
}
