using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class PurchasableSpell : IEquatable<PurchasableSpell>
    {
        public Button PurchaseButton { get; set; }

        public Text CostText { get; set; }

        public KeyCode Key { get; set; }

        public bool IsDisabled { get; set; }

        public PurchasableSpell(Button btn, Text text, KeyCode key)
        {
            PurchaseButton = btn;
            CostText = text;
            Key = key;
            IsDisabled = false;
        }

        public override int GetHashCode()
        {
            return PurchaseButton.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as PurchasableSpell);
        }

        public bool Equals(PurchasableSpell other)
        {
            return other != null && other.PurchaseButton == this.PurchaseButton;
        }
    }
}
