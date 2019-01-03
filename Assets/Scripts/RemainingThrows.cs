using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingThrows : MonoBehaviour {

	public static int remainingNum = 6;
	Text number;

	// Use this for initialization
	void Start () {
		number = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		number.text = remainingNum.ToString();
	}
}
