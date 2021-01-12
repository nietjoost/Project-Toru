using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Key : Item
{

	// Key access is defined by color
	public CardreaderColor color = CardreaderColor.Yellow;

	void Start()
	{

		SpriteRenderer renderer = GetComponentInParent<SpriteRenderer>();

		switch (color)
		{
			case CardreaderColor.Blue:
				renderer.color = ColorZughy.cyan;
				break;
			case CardreaderColor.Yellow:
				renderer.color = ColorZughy.yellow;
				break;
			case CardreaderColor.Purple:
				renderer.color = ColorZughy.purple;
				break;
			case CardreaderColor.Disabled:
				Debug.LogError("KEy can't be 'Disabled'");
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.isTrigger)
		{
			collision.GetComponent<Character>().inventory.addItem(this);
			gameObject.SetActive(false);
			LevelManager.emit("PlayerFoundKey", collision.GetComponent<Character>().gameObject);
		}
	}
}
