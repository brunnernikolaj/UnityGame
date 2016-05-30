using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used to set tilling on a Quad based on scale
/// </summary>
public class TileStretcher : MonoBehaviour {

    Renderer texture;

	void Start () {
        texture = GetComponent<Renderer>();
	}
	
	void Update () {
        texture.material.mainTextureScale = transform.localScale / 10;
	}
}
