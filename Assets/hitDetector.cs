using UnityEngine;
using System.Collections;

public class hitDetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnCollisionEnter(Collision collision) {
        if(collision.collider.name == "sword")
        Debug.Log("HIT!!!");
        }
    // Update is called once per frame
    void Update()
    {
    } 
}
