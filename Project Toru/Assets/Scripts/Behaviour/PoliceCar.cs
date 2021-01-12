using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCar : Car
{
	[Range (0, 10)]
	public int SpawnCops = 1;
	
	public Police policePrefab = null;
	public PoliceCar() {
		base.callback = (Vector3 target) => {
			if (policePrefab == null) {
				Debug.LogError("policePrefab not set");
				return;
			}
			
			for (int i = 0; i < SpawnCops; i++) {
				StartCoroutine(SpawnCop(target));
			}
		};
	}
	
	IEnumerator SpawnCop(Vector3 target)
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        
		Vector3 spawnPosition = target + new Vector3(Random.Range(0f, 1f), 2.0f, 0);
		var cop = Instantiate(policePrefab, spawnPosition, Quaternion.identity);
		PoliceForce.getInstance().AddCop(cop);
    }
	
}
