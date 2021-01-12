using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Aggressive : PoliceState
{
	static Aggressive instance;
	public static Aggressive getInstance()
	{
		if (instance == null)
			instance = new Aggressive();
		return instance;
	}

	public override void Enter()
	{
	}

	public override void Execute()
	{
	}

	public override void Exit()
	{
	}

	public override void MoveCop(Police p)
	{
		// remove this room from lastknownpositions
		if(p.currentRoom !=null)
			LastKnownPositions.Remove(p.currentRoom);

		// give new destination
		if(LastKnownPositions.Count != 0)
			p.setPos(LastKnownPositions.First.Value);
		else
		{
			// search random rooms
			var rooms = GameObject.FindObjectsOfType<Room>();
			p.setPos(rooms[UnityEngine.Random.Range(0, rooms.Length)]);
		}
	}
}
