using UnityEngine;
using System.Collections;

public class TileStretcher : MonoBehaviour {

    Renderer texture;
	// Use this for initialization
	void Start () {
        texture = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        texture.material.mainTextureScale = transform.localScale / 10;
	}
}
