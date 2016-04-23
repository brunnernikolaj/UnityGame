using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using Assets.Scripts.Spells;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using System;

public class ShopMenu : MonoBehaviour
{

    public Text GoldText;

    public Button FireballUpgradeBtn;
    public Text FireballPrice;

    public Button TeleportUpgradeBtn;
    public Text TeleportPrice;

    public Button DashUpgradeBtn;
    public Text DashPrice;

    private Canvas ShopUi;

    private Player localPlayer;

    private Dictionary<PurchasableSpell,ISpell> purchasableSpells = new Dictionary<PurchasableSpell, ISpell>();

    // Use this for initialization
    void Start()
    {
        ShopUi = GetComponent<Canvas>();
        localPlayer = FindObjectOfType<GameManager>().GetLocalPlayer();
        //localPlayer = new Player { Gold = 50 };
        //localPlayer.Spells = new Dictionary<KeyCode, ISpell>();
        //localPlayer.Spells.Add(KeyCode.A, new FireballSpell());

        purchasableSpells.Add(new PurchasableSpell(FireballUpgradeBtn, FireballPrice, KeyCode.A),new FireballSpell());
        purchasableSpells.Add(new PurchasableSpell(TeleportUpgradeBtn, TeleportPrice, KeyCode.W),new TeleportSpell());
        purchasableSpells.Add(new PurchasableSpell(DashUpgradeBtn, DashPrice, KeyCode.W), new DashSpell());

        foreach (var spell in purchasableSpells.Keys)
        {
            var tempSpell = spell;
            spell.PurchaseButton.onClick.AddListener(() => BuySpell(tempSpell));
        }

        GoldText.text = localPlayer.Gold.ToString();

        foreach (var spellUi in purchasableSpells.Keys)
        {
            if (!spellUi.IsDisabled)
            {
                spellUi.PurchaseButton.interactable = localPlayer.Gold > purchasableSpells[spellUi].UpgradeCost;
                spellUi.CostText.text = purchasableSpells[spellUi].UpgradeCost + "G";
            }
        }
    }

    public void BuySpell(PurchasableSpell spell)
    {
        if (!localPlayer.Spells.ContainsKey(spell.Key))
        {
            localPlayer.Spells.Add(spell.Key, purchasableSpells[spell]);
        }

        localPlayer.Gold -= localPlayer.Spells[spell.Key].UpgradeCost;
        localPlayer.Spells[spell.Key].UpgradeSpell();
        spell.CostText.text = localPlayer.Spells[spell.Key].UpgradeCost.ToString() + "G";

        var tobeDisabled = purchasableSpells.Keys.Where(x => x.Key == spell.Key).ToList();
        tobeDisabled.Remove(spell);

        foreach (var tempSpell in tobeDisabled)
        {
            tempSpell.IsDisabled = true;
            tempSpell.PurchaseButton.interactable = false;
        }

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        foreach (var spellUi in purchasableSpells.Keys)
        {
            if (!spellUi.IsDisabled)
            {
                spellUi.PurchaseButton.interactable = localPlayer.Gold > localPlayer.Spells[spellUi.Key].UpgradeCost;
                spellUi.CostText.text = localPlayer.Spells[spellUi.Key].UpgradeCost + "G";
            }      
        }

        //Update player gold
        GoldText.text = localPlayer.Gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShopUi.enabled = !ShopUi.enabled;
        }
    }

}
