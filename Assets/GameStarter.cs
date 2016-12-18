using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {


	// Use this for initialization
	void Start () {
		timerStart ();
	}

	public void timerStart() {
		InvokeRepeating ("Countdown", 1.0f, 1.0f);
	}

	public void startGame() {
		SceneManager.LoadScene ("B5main");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
