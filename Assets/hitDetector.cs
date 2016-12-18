using UnityEngine;
using System.Collections;

public class hitDetector : MonoBehaviour {
    public GameObject blood;
    bool dead = false;
	// Use this for initialization
	void Start () {
	
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "sword")
        {
            ParticleSystem ps = blood.GetComponent<ParticleSystem>();
            if (!dead)
            {
                ps.Play();
            }
            Debug.Log("HIT!!!");
            this.GetComponent<Animator>().SetTrigger("B_Dying");
            //this.GetComponent<Animator>().SetBool("Dead", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Animator anim = this.GetComponent<Animator>();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            anim.SetBool("Dead", true);
            dead = true;
        }
    } 
}
