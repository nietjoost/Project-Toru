using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	private IState currentlyRunningState;
	private IState previousState;

	public virtual void ChangeState(IState newState)
	{
		if(currentlyRunningState != null)
		{
			this.currentlyRunningState.Exit();
		}

		this.previousState = this.currentlyRunningState;
		this.currentlyRunningState = newState;
		this.currentlyRunningState.Enter();
	}

	public virtual void ExecuteStateUpdate()
	{
		var runningState = this.currentlyRunningState;
		if(runningState != null)
		{
			runningState.Execute();
		}
	}

	public virtual void SwitchToPreviousState()
	{
		this.currentlyRunningState.Exit();
		this.currentlyRunningState = previousState;
		this.currentlyRunningState.Enter();
	}

	public virtual IState GetCurrentlyRunningState()
	{
		return currentlyRunningState;
	}
}
