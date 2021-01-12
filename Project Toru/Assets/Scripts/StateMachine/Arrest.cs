using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrest : IState
{
    private Weapon weapon;
    private GameObject gameObject;
    private GameObject firePoint;
    private Animator animator;
    private GameObject target;
	private NPC npc;

	float timer = 0.5f;
	bool moving = false;

    public Arrest(NPC npc, Weapon weapon, GameObject gameObject, GameObject firePoint, Animator animator, GameObject target)
    {
        this.weapon = weapon;
        this.gameObject = gameObject;
        this.firePoint = firePoint;
        this.animator = animator;
		this.target = target;
		this.npc = npc;
    }

    public void Enter()
    {
		npc.Say("Stand still!");
    }

    public void Execute()
    {	
		timer -= Time.deltaTime;
		if (timer <= 0) {
			Move();
			timer = 0.5f;
		}

		if (!animator.GetBool("moving")) {
			CheckTargetDirection();
			AdjustFirePoint();
		}

		if (target.activeSelf == false) {
			npc.StopShooting();
		}
        
    }

    public void Exit()
    {
		target.GetComponent<Character>().StopSurrender();
    }

	void Move() {

		Vector3 distance = target.transform.position - gameObject.transform.position;
		
		if (Mathf.Abs(distance.x) > 4 || Mathf.Abs(distance.y) > 0.5) {
			Vector3 target = this.target.transform.position;
			if (distance.x > 0) target.x -= 2;
			else				target.x += 2;

			gameObject.GetComponent<ExecutePathFindingNPC>().setPosTarget(target);
			return;
		}

		
		target.GetComponent<Character>().Surrender();
	}

    void CheckTargetDirection()
    {
        Vector3 distance = target.transform.position - gameObject.transform.position;
		if (distance.x <= 0) {
			animator.SetFloat("moveX", -1f);
		} else {
			animator.SetFloat("moveX", 1f);
		}
    }

    void AdjustFirePoint()
    {
        if (animator.GetFloat("moveX") > 0)
        {
            firePoint.transform.rotation = Quaternion.Euler(0, 0, 0);
            firePoint.transform.position = gameObject.transform.position + new Vector3(.3f, -.3f);
            firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
        }
        else
        {
            firePoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            firePoint.transform.position = gameObject.transform.position + new Vector3(-.3f, -.3f);
            firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
        }

        if (animator.GetFloat("moveY") > 0.1)
        {
            weapon.HideGun();
        }
        else
        {	
            weapon.RevealGun();
        }
    }
}
