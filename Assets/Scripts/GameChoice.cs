using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameChoice : MonoBehaviour {

	// Use this for initialization
	public void LoadLevelNormal () {
		SceneManager.LoadScene(1);
	}

	public void LoadLevelExtreme () {
		SceneManager.LoadScene(2);
	}

	public void LoadMenu () {
		SceneManager.LoadScene(0);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
