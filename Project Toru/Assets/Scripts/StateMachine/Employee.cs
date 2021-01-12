using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : NPC

{ 
    protected override void Start()
    {
		base.Update();
    }

    protected override void Update()
    {
		base.Update();

        FleeIfPossible();
    }

    public override void Surrender() {
		base.Surrender();

		if (currentRoom.SelectedPlayerInRoom() && !surrender) {
			int random = Random.Range(0, 3);
			switch (random) {
				case 0:
					Say("Not me!");
				break;

				case 1:
					Say("I have nothing!");
				break;

				case 2:
					Say("Let me live!");
				break;

				case 3:
					Say("Don't shoot!");
				break;

				default:
				break;
			}
		}

		dropBag();
		
	}
}
