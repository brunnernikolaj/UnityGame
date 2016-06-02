using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpellShopIcon : MonoBehaviour {

    private Image icon;
    private Text priceText;

	// Use this for initialization
	void Awake () {
        icon = GetComponent<Image>();
        priceText = GetComponentInChildren<Text>();
    }
	
    public void init(int price , string iconName)
    {
        priceText.text = price + " G";
        icon.sprite = Resources.Load<Sprite>("SpellSprites/" + iconName);
    }

    public void UpdatePrice(int price)
    {
        priceText.text = price + " G";
    }
}
