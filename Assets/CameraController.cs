using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    float speed;
	// Use this for initialization
	void Start () {
        speed = 10f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 newPos = new Vector3(0, 0, this.transform.forward.z * speed * Time.deltaTime);
            transform.position += newPos;
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 newPos = new Vector3(0, 0, this.transform.forward.z * speed * Time.deltaTime);
            transform.position -= newPos;
        }
    }
}
