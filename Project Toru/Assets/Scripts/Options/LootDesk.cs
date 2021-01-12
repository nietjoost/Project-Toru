using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Options
{
    public class LootDesk : Option
    {
        public GameObject Key = null;

        private Desk desk;

        public void Start()
        {
            desk = GetComponentInParent<Desk>();
        }

        public override string Activate(Character c)
        {
            c.inventory.addItem(Key.GetComponent<Key>());
			LevelManager.emit("PlayerFoundKey", c.gameObject);
            return null;
        }
    }
}
