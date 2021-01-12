using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    
	public Room room = null;
	
	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.isTrigger)
		{
			if (other.CompareTag("Player"))
			{
				LevelManager.emit("CameraDetectedPlayer", room.gameObject);
			}
		}
	}
}
