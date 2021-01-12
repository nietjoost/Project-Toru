using System.Collections.Generic;
using UnityEngine;

public class Surrender : IState
{
	private Animator animator;

	public Surrender(Animator animator)
	{
		this.animator = animator;
	}

	public void Enter()
	{
		animator.SetBool("Surrendering", true);
	}

	public void Execute()
	{

	}

	public void Exit()
	{
		animator.SetBool("Surrendering", false);
	}
}
