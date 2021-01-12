using System;
using Assets.Scripts.Options;
using System.Collections.Generic;
using UnityEngine;

public class Muscle : Character
{

	


	protected override void FlipFirePoint() {
		GameObject firePoint = weapon.gameObject;

		if (animator.GetFloat("moveX") > 0.1)
		{
			firePoint.transform.rotation = Quaternion.Euler(0, 0, 0);
			firePoint.transform.position = transform.position + new Vector3(.3f, -0.4f);
			firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
		}

		if (animator.GetFloat("moveX") < -0.1)
		{
			firePoint.transform.rotation = Quaternion.Euler(0, 180, 0);
			firePoint.transform.position = transform.position + new Vector3(-.3f, -0.4f);
			firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
		}
	}
}