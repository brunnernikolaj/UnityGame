using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This class is used to reduce the size of a gameobject over time
/// </summary>
public class FloorReducer : MonoBehaviour {

    public float ShrinkAmount;

    public float TimeBetweenShrink;
    private float timeRemaingBeforeShrink;

	void Start () {
        timeRemaingBeforeShrink = TimeBetweenShrink;
    }
	
	void Update () {

        //We want to have a little bit of space left to stand on
        if (transform.localScale.x > 10)
        {
            if (timeRemaingBeforeShrink > 0)
            {
                timeRemaingBeforeShrink -= Time.deltaTime;
            }
            else
            {
                Shrink();
                timeRemaingBeforeShrink = TimeBetweenShrink;
            }
        }       
	}

    private void Shrink()
    {
        var newScaleValue = new Vector3(transform.localScale.x - ShrinkAmount, transform.localScale.y - ShrinkAmount);
        LeanTween.scale(gameObject, newScaleValue, 0f );
    }
}
