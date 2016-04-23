using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FollowPlayerHpBar : MonoBehaviour {

    public GameObject player { get; set; }

    public GameObject HpBar;

    private Image redBar;
    private Image greenBar;

	// Use this for initialization
	void Start () {
        var images = GetComponentsInChildren<Image>();

        redBar = images[0];
        greenBar = images[1];
       
	}

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x - 2.5f,player.transform.position.y - 2f);
	}

    public void SetHealth(float updatedHealth)
    {
        HpBar.transform.localScale = new Vector3(updatedHealth / 100f, 1f);
    }

    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(2f);

        redBar.CrossFadeAlpha(0f, 1f, false);
        greenBar.CrossFadeAlpha(0f, 1f, false);
    }

    public void Show()
    {
        redBar.CrossFadeAlpha(1f, 0.1f, false);
        greenBar.CrossFadeAlpha(1f, 0.1f, false);

        StartCoroutine(Fade());
    }
}
