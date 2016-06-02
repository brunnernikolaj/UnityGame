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

    public List<GameObject> spells;

    public Transform AList;
    public Transform WList;

    public GameObject IconPrefab;

    private Dictionary<KeyCode, List<GameObject>> buttons = new Dictionary<KeyCode, List<GameObject>>();

    private Canvas ShopUi;

    private Player localPlayer;


    // Use this for initialization
    void Start()
    {
        ShopUi = GetComponent<Canvas>();
        localPlayer = FindObjectOfType<GameManager>().GetLocalPlayer();

        var spell = localPlayer.Spells[KeyCode.A];
        var go = Instantiate(IconPrefab);

        go.GetComponent<SpellShopIcon>().init(spell.UpgradeCost, spell.IconName);
        go.GetComponent<Button>().onClick.AddListener(() => BuySpell(spell, go, KeyCode.A));

        go.transform.SetParent(AList.transform);
        buttons.Add(KeyCode.A, new List<GameObject> { go });

        List<ISpell> wSpells = new List<ISpell>
            {
                new TeleportSpell(),
                new DashSpell()
            };

        SetupSpellCategorey(KeyCode.W, WList, wSpells);

        GoldText.text = localPlayer.Gold.ToString();

        //foreach (var spellUi in purchasableSpells.Keys)
        //{
        //    if (!spellUi.IsDisabled)
        //    {
        //        spellUi.PurchaseButton.interactable = localPlayer.Gold > purchasableSpells[spellUi].UpgradeCost;
        //        spellUi.CostText.text = purchasableSpells[spellUi].UpgradeCost + "G";
        //    }
        //}
    }

    private void SetupSpellCategorey(KeyCode keyCode, Transform parentContainer, List<ISpell> spells)
    {
        if (localPlayer.Spells.ContainsKey(keyCode))
        {
            var spell = localPlayer.Spells[keyCode];

            var go = Instantiate(IconPrefab);

            go.GetComponent<SpellShopIcon>().init(spell.UpgradeCost, spell.IconName);
            go.GetComponent<Button>().onClick.AddListener(() => BuySpell(spell, go, keyCode));
            go.transform.SetParent(WList.transform);

            if (buttons.ContainsKey(keyCode))
            {
                buttons[keyCode].Add(go);
            }
            else
            {
                buttons.Add(keyCode, new List<GameObject> { go });
            }
        }
        else
        {
            foreach (var spell in spells)
            {
                var tempSpell = spell;
                var go = Instantiate(IconPrefab);
                var icon = go.GetComponent<SpellShopIcon>();
                var button = go.GetComponent<Button>();

                icon.init(spell.UpgradeCost, spell.IconName);

                button.onClick.AddListener(() => BuySpell(tempSpell, go, keyCode));
                go.transform.SetParent(WList.transform);

                if (buttons.ContainsKey(keyCode))
                {
                    buttons[keyCode].Add(go);
                }
                else
                {
                    buttons.Add(keyCode, new List<GameObject> { go });
                }
            }
        }
    }

    public void BuySpell(ISpell spell, GameObject icon, KeyCode keyCode)
    {
        if (!localPlayer.Spells.ContainsKey(keyCode))
        {
            localPlayer.Spells.Add(keyCode, spell);
        }

        localPlayer.Gold -= localPlayer.Spells[keyCode].UpgradeCost;
        localPlayer.Spells[keyCode].UpgradeSpell();

        icon.GetComponent<SpellShopIcon>().UpdatePrice(localPlayer.Spells[keyCode].UpgradeCost);

        var tobeDisabled = new List<GameObject>( buttons[keyCode]);

        if (spell.Level < 3)
        {
            tobeDisabled.Remove(icon);
        }

        //Disable all spells that use the same key
        foreach (var tempSpell in tobeDisabled)
        {
            Destroy(tempSpell);
        }

        GoldText.text = localPlayer.Gold.ToString();
        //UpdateButtons();
    }

    private void UpdateButtons()
    {
        //Update buttons
        foreach (var spellUi in localPlayer.Spells)
        {
            buttons[spellUi.Key].First().GetComponent<SpellShopIcon>().UpdatePrice(spellUi.Value.UpgradeCost);
        }

        //Update player gold
        GoldText.text = localPlayer.Gold.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShopUi.enabled = !ShopUi.enabled;
        }
    }
}
