using UnityEngine;
using System.Collections;

public class ScoreboardController : Singleton<ScoreboardController> {

    public GameObject scoreEntryPrefab;

	// Use this for initialization
	void Start () {
        foreach (var player in GameManager.Instance.GetPlayers())
        {
            var go = (GameObject)Instantiate(scoreEntryPrefab);
            go.transform.SetParent(this.transform);
        }
	}


}
