using Assets.Scripts.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutePathFindingPlayable : ExecutePathFinding
{
	public bool disabled = false;

	public void Update()
	{
		if (disabled) {
			return;
		}

		MousePointInput();
		WayPointsWalk();
		HidePlayerOnStair();
		AdjustOrderLayer();
	}

	private void MousePointInput()
	{
		if(GetComponent<Character>().Equals(Character.selectedCharacter)) {
			
			if (Input.GetMouseButtonDown(1))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Plane plane = new Plane(Vector3.forward, Character.selectedCharacter.transform.position);
				float dist = 10;
				
				LevelManager.emit("CharacterHasBeenMoved");

				if (plane.Raycast(ray, out dist))
				{
					Vector2 pos = ray.GetPoint(dist);

					targetFurniture = null;

					PathFinding(pos);
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		checkDoorClosed(other);

		if (other.CompareTag("Room"))
		{
			currentRoom = other.gameObject.GetComponent<Room>();
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		checkDoorClosed(other);
	}
}