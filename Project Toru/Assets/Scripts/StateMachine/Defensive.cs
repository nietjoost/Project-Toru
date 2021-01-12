using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Defensive : PoliceState
{
	static Defensive instance;
	public static Defensive getInstance()
	{
		if (instance == null)
			instance = new Defensive();
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
		if (entrances.Count != 0)
		{
			var entrancePosition = entrances.Last().transform.position;
			p.GetComponent<ExecutePathFindingNPC>().setPosTarget(new Vector3(UnityEngine.Random.Range(entrancePosition.x - 1.0f, entrancePosition.x + 1.0f), UnityEngine.Random.Range(entrancePosition.y - 1.0f, entrancePosition.y + 1.0f), entrancePosition.z));
		}
	}
}
