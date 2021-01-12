using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
	
	Animator animator = null;
	
	public GameObject _target = null;
	public GameObject target {
		get {
			return _target;
		}
		set {
			_target = value;
			DriveAnimation(true);
		}
	}
	
	[Range (0f, 10f)]
	public float target_speed = 5;
	
	[Range (0f, 10f)]
	public float speed = 0;
	
	bool directionRight = false;
	
	public delegate void CarReachTargetCallback(Vector3 target);
	public CarReachTargetCallback callback = null;
	
    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
		
		// Turn on "driving"
		if (target != null) {
			DriveAnimation(true);
		}
		
		// Flip driving direction when artwork is flipped
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		directionRight = !spriteRenderer.flipX;
    }
	
	void Update() {
		if (target != null) {
			if ((int) target.transform.position.x != (int) transform.position.x) {
				// We only 'drive' in X direction
				transform.Translate(new Vector3(directionRight ? speed : -speed, 0, 0) * Time.deltaTime);
			} else {
				callback.Invoke(target.transform.position);
				target = null;
				DriveAnimation(false);
			}
		}
		
		
	}
	
	void DriveAnimation(bool enable) {
		animator?.SetBool("driving", enable);
	}
	
	
	public void Drive() {
		speed = target_speed;
	}
}
