using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutePathFindingNPC : ExecutePathFinding
{
	private void Update()
	{
		WayPointsWalk();
		HidePlayerOnStair();
		AdjustOrderLayer();
	}

	public void setPosTarget(Vector3 pos)
	{
		PathFinding(pos);
	}

	public void setPosTarget(float x, float y)
	{
		PathFinding(new Vector3(x, y, 0));
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
