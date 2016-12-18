using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextSetter : MonoBehaviour {

	public Text results;

	// Use this for initialization
	void Start () {
		int kills = PlayerPrefs.GetInt ("Kills");
		string result = "\n test";
		if (kills < 6) {
			result = "\n That is not a good job";
		} else if (kills < 11) {
			result = "\n That is adequate";
		} else {
			result = "\n Good job getting revenge";
		}
		results.text = "You killed " + kills + " humans " + result;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
