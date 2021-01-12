using UnityEngine;

public class PingPong : IState
{
	private Vector3 startPos;
	private Vector3 change;
	private GameObject gameObject;
	private Animator animator;


	public PingPong(Vector3 startPos, GameObject gameObject, Animator animator)
	{
		this.startPos = startPos;
		this.gameObject = gameObject;
		this.animator = animator;
	}

	public void Enter()
	{
	}

	public void Execute()
	{
		
		var trans = gameObject.GetComponent<Transform>();
		change = new Vector3(Mathf.PingPong(Time.time, 1.5f) + startPos.x, trans.position.y, trans.position.z);

		if(trans.position != change)
		{
			animator.SetBool("moving", true);
			animator.SetFloat("moveX", change.x - trans.position.x);
			animator.SetFloat("moveY", change.y - trans.position.y);
			trans.position = change;
		}

	}

	public void Exit()
	{
		animator.SetBool("moving", false);
		animator.SetFloat("moveX", 0);
		animator.SetFloat("moveY", 0);
	}
}
