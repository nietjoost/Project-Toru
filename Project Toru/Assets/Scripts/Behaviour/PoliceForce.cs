using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PoliceForce : StateMachine//: MonoBehaviour
{
	List<Police> Cops = new List<Police>();
	
	static PoliceForce instance;

	public static PoliceForce getInstance()
	{
		if (instance == null)
			instance = new PoliceForce();
		return instance;
	}

	//	public void Start()
	private PoliceForce()
	{
		ChangeState(Defensive.getInstance());
	}

	public void Alert(Room seen)
	{
		LevelManager.emit("CopsTriggered");
		if(((PoliceState)GetCurrentlyRunningState()).AddPosition(seen) && !(GetCurrentlyRunningState() is Defensive))
			foreach (var p in Cops)
				RequestOrders(p);
	}

	public void AlertKill()
	{
		ChangeState(Aggressive.getInstance());
	}

	public void AddCop(Police cop)
	{
		Cops.Add(cop);
	}

	public void RequestOrders(Police cop)
	{
		if(!(cop.statemachine.GetCurrentlyRunningState() is Combat))
		((PoliceState)GetCurrentlyRunningState()).MoveCop(cop);
	}
}