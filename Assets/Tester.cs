using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int layerMask = 1 << 8;

        var one = new Vector2(11, 38);
        var two = new Vector2(21,21);

        var direction = one - two;
        var distance = Vector2.Distance(one, two);


        var hit = Physics2D.CircleCast(one, 3f, direction, direction.magnitude, layerMask);
       

        Debug.Log(hit.collider != null);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
