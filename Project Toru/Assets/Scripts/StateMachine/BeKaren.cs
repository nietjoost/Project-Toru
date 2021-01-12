using UnityEngine;
using System.Collections.Generic;

public class BeKaren : IState
{
    private Animator animator;
	private SpriteSelector spriteSelector;
	private GameObject target;
	private GameObject gameObject;
	private GameObject firePoint;
	private NPC npc;
	private Weapon weapon;

	float timer = 0.5f;
	float textTimer = 5;

	List<string> lines = new List<string>();
	int currentLine = 0;

    public BeKaren(Karen npc, Character target, GameObject firepoint, Weapon weapon)
    {
        this.animator = npc.animator;
		this.spriteSelector = npc.GetComponent<SpriteSelector>();
		this.gameObject = npc.gameObject;
		this.firePoint = firepoint;
		this.npc = npc;
		this.weapon = weapon;
		this.target = target.gameObject;

		lines.Add("I HATE YOU");
		lines.Add("I HAVE NO LIFE THANKS TO YOU");
		lines.Add("BLA BLA BLA BLA BLA");
		lines.Add("YOU ARE GOING TO DIE");
		lines.Add("WHY WON'T YOU DIE??");
		lines.Add("AAARRHHGG");
    }

    public void Enter()
    {
		spriteSelector.SpriteSheetName = "karen_red";
    }

    public void Execute()
    {
		timer -= Time.deltaTime;
		textTimer -= Time.deltaTime;
		if (timer <= 0) {
			Move();
			timer = 0.5f;
		}

		if (textTimer <= 0) {
			if (currentLine < lines.Count) {
				npc.Say(lines[currentLine]);
				currentLine++;
				textTimer = 5;
			}
		}

		if (!animator.GetBool("moving")) {
			CheckTargetDirection();
			AdjustFirePoint();
			weapon.Shoot();
		}

		if (target.activeSelf == false) {
			npc.StopShooting();
		}
    }

    public void Exit()
    {
		spriteSelector.SpriteSheetName = "karen";
    }

	void Move() {

		Vector3 distance = target.transform.position - gameObject.transform.position;
		
		if (Mathf.Abs(distance.x) > 4 || Mathf.Abs(distance.y) > 0.5 || Mathf.Abs(distance.x) < 1) {
			Vector3 target = this.target.transform.position;
			if (distance.x > 0) target.x -= 2;
			else				target.x += 2;

			gameObject.GetComponent<ExecutePathFindingNPC>().setPosTarget(target);
		}
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
            firePoint.transform.position = gameObject.transform.position + new Vector3(.3f, -.4f);
            firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
        }
        else
        {
            firePoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            firePoint.transform.position = gameObject.transform.position + new Vector3(-.3f, -.4f);
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
