using UnityEngine;

public class Idle : IState
{
    private Animator animator;


    public Idle(Animator animator)
    {
        this.animator = animator;
    }

    public void Enter()
    {
        animator.SetBool("moving", false);
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", 0);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
