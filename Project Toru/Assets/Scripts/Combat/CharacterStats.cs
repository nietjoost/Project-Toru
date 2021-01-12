using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{	
	public float _maxHealth;
    public float maxHealth {
		set {
			_maxHealth = value;
			currentHealth = value;	
		}
		get{
			return _maxHealth;
		}
	}

    [NonSerialized]
    public float currentHealth;



    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            NPC npc = this.gameObject.GetComponent<NPC>();
            if(npc != null)
            {
                npc.dropBag();
			    LevelManager.emit("NPCKilled", npc.currentRoom.gameObject);
            }
			
			LevelManager.emit("Killed", gameObject);
            gameObject.SetActive(false);
        }
    }
}
