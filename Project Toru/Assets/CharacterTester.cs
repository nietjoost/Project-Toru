using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTester : MonoBehaviour
{

    public List<GameObject> Characters = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Let other characters init first
        StartCoroutine(LateStart(0.01f));
    }


    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Resetting all characters to "idle" when needed"
        foreach (var character in Characters)
        {

            Employee employee = character.GetComponent<Employee>();

            if (employee != null)
            {
                if (employee.animator != null)
                {
                    Debug.Log("Setting idle!");
                    employee.statemachine.ChangeState(new Idle(employee.animator));
                }
            }
        }


    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (var character in Characters)
        {
            Animator animator = character.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Surrendering", true);
                animator.SetBool("moving", true);
                animator.SetFloat("moveY", -1);
                animator.SetFloat("moveX", -1);
                animator.SetBool("Surrendering", true);
            }
        }
    }
}
