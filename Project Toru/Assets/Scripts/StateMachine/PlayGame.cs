using UnityEngine;

public class PlayGame : IState
{
	private FatGuy fatGuy;

	float timer = 2;
	int state = 0;

    public PlayGame(FatGuy fatGuy)
    {	
		this.fatGuy = fatGuy;
    }

    public void Enter()
    {
        fatGuy.animator.SetBool("moving", false);
        fatGuy.animator.SetFloat("moveX", 1);
        fatGuy.animator.SetFloat("moveY", 0);
    }

    public void Execute()
    {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			timer = 2;
			
			switch (state) {
				case 1:
					fatGuy.Say("Burb");
				break;

				case 2:
					fatGuy.animator.SetBool("Surrendering", true);
				break;

				case 3:
					fatGuy.animator.SetBool("Surrendering", false);
				break;

				case 5:
					fatGuy.Say("Yeah!");
				break;

				case 7:
					fatGuy.Say("Buhjah!");
				break;

				case 8:
					state = 0;
				break;

			}

			state ++;
		}

    }

    public void Exit()
    {
    }
}
