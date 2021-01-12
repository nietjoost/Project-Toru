using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatGuy : NPC
{
    // Start is called before the first frame update
    protected override void Start()
    {
		base.Start();
        
		statemachine.ChangeState(new PlayGame(this));
    }
	
	protected override void Update()
    {
		base.Update();

		FleeIfPossible();
    }

	public void RunAway() {
		this.pathfinder.setPosTarget(-30, 1);
		statemachine.ChangeState(new Idle(this.animator));
		animator.SetBool("Surrendering", true);
	}
}
