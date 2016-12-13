using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotY = Input.GetAxis("Mouse X") * 3.0f;
        if (Input.mousePosition.x < 30 && rotY<0)
        {
            transform.Rotate(0, rotY, 0);
        }
        if (Input.mousePosition.x > Screen.width - 30 && rotY>0)
        {
            transform.Rotate(0, rotY, 0);
        }

        
        float zForce = Input.GetAxis("Vertical");
        float xForce = Input.GetAxis("Horizontal");
        anim.SetFloat("zForce", zForce);
        anim.SetFloat("xForce", xForce);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
        if (Input.GetKey(KeyCode.LeftControl) && xForce != 0)
        {
            anim.SetBool("strafe", true);
        }
        else
        {
            if (zForce == 0 && xForce != 0)
            {
                transform.Rotate(new Vector3(0, xForce * 60 * Time.deltaTime));
            }
            anim.SetBool("strafe", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("altAttack");
        }
    }
}
