using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour {

	[SerializeField]
	GameObject baton;

	public void Spawn()
	{
		var NewBaton = Instantiate (baton, new Vector3(0f, -0.161f, 0.499f), Quaternion.identity);
		NewBaton.transform.parent = this.gameObject.transform;
	}
}
