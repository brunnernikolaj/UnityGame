using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FollowPlayerHpBar : MonoBehaviour {

    //The object to follow
    public GameObject player { get; set; }

    public GameObject HpBar;

    private Image redBar;
    private Image greenBar;

	void Start () {
        var images = GetComponentsInChildren<Image>();

        redBar = images[0];
        greenBar = images[1];       
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x - 2.5f,player.transform.position.y - 2f);
	}

    public void SetHealth(float updatedHealth)
    {
        HpBar.transform.localScale = new Vector3(updatedHealth / 100f, 1f);
    }

    /// <summary>
    /// Fade out the healthbar
    /// </summary>
    /// <returns></returns>
    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(2f);
        if (redBar != null)
        {
            redBar.CrossFadeAlpha(0f, 1f, false);
            greenBar.CrossFadeAlpha(0f, 1f, false);
        }
    }

    /// <summary>
    /// Show the healthbar
    /// </summary>
    public void Show()
    {
        redBar.CrossFadeAlpha(1f, 0.1f, false);
        greenBar.CrossFadeAlpha(1f, 0.1f, false);

        StartCoroutine(Fade());
    }
}
