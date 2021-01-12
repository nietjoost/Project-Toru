using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Options
{
	public class LootVault : Option
	{
		public GameObject money = null;
        
        private Vault Vault;
        private CardreaderColor color;

		public void Start()
		{
			Vault = GetComponentInParent<Vault>();
            color = Vault.keycardColor;
		}

		public override string Activate(Character C)
		{
            if ((color == CardreaderColor.Disabled) || (C.HasKey(color)))
            {
                Vault.Open();
				LevelManager.emit("CharacterGotMoneyFromVault");
                C.inventory.addItem(money.GetComponent<Money>());
                return null;
            }
            else
            {
                // Dialog box to tell the player they need a [color] keycard
                return color + " keycard required to open this vault";
            }
		}
	}
}
