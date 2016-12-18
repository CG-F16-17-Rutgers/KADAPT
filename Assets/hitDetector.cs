using UnityEngine;
using System.Collections;

public class hitDetector : MonoBehaviour {
    public TextMesh text;
    public TextMesh totalKilled;
    public GameObject blood;
    public static bool panic = false;
    bool dead = false;
    public static int total = 0;
	// Use this for initialization
	void Start () {
        text = text.GetComponent<TextMesh>();
        text.text = "";
        totalKilled = totalKilled.GetComponent<TextMesh>();
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
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
		if (Physics.Raycast(transform.position, fwd, out hit, 100) && hit.collider.name == "Goblin_D_Shareyko" && dead == false)
        {
            Debug.Log("Player spotted!");
            text.text = "Spotted!";
            panic = true;
            
        }
        Animator anim = this.GetComponent<Animator>();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            
            if (!dead)
            {
                total++;
                totalKilled.text = "Humans Killed: " + total.ToString();
            }
            anim.SetBool("Dead", true);
            dead = true;
            
     
        }
    }
     
}



