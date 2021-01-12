using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class PoliceState : IState
{
	protected static LinkedList<Room> LastKnownPositions = new LinkedList<Room>();
	protected static List<GameObject> entrances;

	public PoliceState()
	{
		if(entrances == null)
		{
			entrances = GameObject.FindGameObjectsWithTag("Entrance").ToList();
		}
	}
	public virtual bool AddPosition(Room room)
	{
		if (LastKnownPositions.Contains(room))
			return false;
		LastKnownPositions.AddFirst(room);
		return true;
	}
	public abstract void Enter();
	public abstract void Execute();
	public abstract void Exit();
	public abstract void MoveCop(Police p);
}