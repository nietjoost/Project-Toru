using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Guard : NPC
{

    
	public Character targetCharacter;
	bool flee = false;

    protected override void Start()
    {	
		base.Start();
        PingPong();
	}

	protected override void Update()
    {	
		base.Update();
        FleeIfPossible();
		
	}

	protected override void FleeIfPossible() {
		if (flee) return;

		if (currentRoom == null) return;

        if (!currentRoom.AnyCharacterInRoom() && surrender)
        {	
			flee = true;
			ShootAt(targetCharacter);
		}
	}

	public override void StopShooting() {
		
		base.StopShooting();
		GetComponent<ExecutePathFindingNPC>().setPosTarget(startingPosition);
	}

	public void Arrest(Character character) {
		if (!surrender)
		this.statemachine.ChangeState(new Arrest(this, weapon, gameObject, firePoint, animator, character.gameObject));
	}
}
