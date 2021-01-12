using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
	private void Start()
	{
		isStackable = true;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.isTrigger)
		{
			collision.GetComponent<Character>().inventory.addItem(this);
			gameObject.SetActive(false);
		}
	}
}

