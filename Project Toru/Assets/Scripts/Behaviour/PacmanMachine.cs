using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PacmanMachine : Item
{

	// Key access is defined by color

	void Start()
	{

		SpriteRenderer renderer = GetComponentInParent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.isTrigger)
		{	
			Character character = collision.GetComponent<Character>();

			if (character.name == "Architect") {
				collision.GetComponent<Character>().inventory.addItem(this);
				gameObject.SetActive(false);
				LevelManager.emit("PlayerFoundPacmanMachine", collision.GetComponent<Character>().gameObject);
			}
			else if (character.name == "Muscle") {
				LevelManager.emit("MuscleFoundPacmanMachine", collision.GetComponent<Character>().gameObject);
			}
			
		}
	}
}
