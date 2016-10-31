using UnityEngine;
using System.Collections;

public class pointSetter : MonoBehaviour {
	public Transform wander1;
	public Transform wander2;
	// Use this for initialization
	void Start () {
		wander1.position = new Vector3(UnityEngine.Random.Range(10, 20), 0, UnityEngine.Random.Range(-10, 20)); 
		wander2.position = new Vector3 (wander1.position.x - 5, wander1.position.y, wander1.position.z);
	}

	void update() {
		
	}

}