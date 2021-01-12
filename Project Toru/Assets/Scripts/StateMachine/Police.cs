using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Police : NPC
{
	[SerializeField]
	Room Dest, lastRoom;

	protected override void Awake() {
		base.Awake();

		/*if (weapon != null)
		{
			weapon.RevealGun();
		}*/
	}

	protected override void Start()
	{
		base.Start();

		weapon.gameObject.transform.position = transform.position + new Vector3(.3f, -.3f);
		PoliceForce.getInstance().RequestOrders(this);
	}

	protected override void Update()
	{	
		if (currentRoom != null && currentRoom.charactersInRoom.Count > 0)
		{
			// GetComponent<ExecutePathFindingNPC>().StopPathFinding();
			PoliceForce.getInstance().Alert(currentRoom);
			if(!(statemachine.GetCurrentlyRunningState() is Combat))
				this.statemachine.ChangeState(new Combat(this, weapon, gameObject, firePoint, animator, currentRoom.charactersInRoom.First().gameObject));
		}
		else if(!(PoliceForce.getInstance().GetCurrentlyRunningState() is Defensive) && !(statemachine.GetCurrentlyRunningState() is Combat) && currentRoom == Dest)
		{
			PoliceForce.getInstance().RequestOrders(this);
		}
		base.Update();
	}

	public void setPos(Room dest)
	{
		Dest = dest;
		GetComponent<ExecutePathFindingNPC>().setPosTarget(Dest.GetPosition() + new Vector3(Dest.GetSize().x/2.0f, 0.5f, 0.0f));
	}

	public override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Room"))
		{
			lastRoom = currentRoom;
			currentRoom = other.gameObject.GetComponent<Room>();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Room") && other.gameObject.GetComponent<Room>() == currentRoom)
		{
			var temp = currentRoom;
			currentRoom = lastRoom;
			lastRoom = temp;
		}
	}
}
