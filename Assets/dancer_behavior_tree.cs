using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class dancer_behavior_tree : MonoBehaviour
{
    public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public Transform wander4;
    public Transform wander5;
    public Transform wander6;
    public Transform wander7;
    public GameObject participant;
	public GameObject police;
	private BehaviorAgent behaviorAgent;
    GameObject[] dancers;
	private float time = 30f;
    // Use this for initialization
    void Start ()
	{
        dancers = GameObject.FindGameObjectsWithTag("dancer");
        behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		timerStart ();
	}

	// Update is called once per frame
	void Update ()
	{
        
    }

	public void timerStart() {
		InvokeRepeating ("Countdown", 1.0f, 1.0f);
	}

	void Countdown () {
		time--;
		if (time == 0) {
			CancelInvoke ("Countdown");
			behaviorAgent.StopBehavior ();
			Debug.Log ("timer expired");
		}
	}

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position); 
        return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position,new Val<float>(2)), new LeafWait(1000));
	}

    protected Node ST_Watch(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(10000));
    }
    
    protected Node ST_BreakDance()
    {
        Debug.Log("trying");
        GameObject[] dancers = GameObject.FindGameObjectsWithTag("dancer");
        bool dance = false;

        dance = (Vector3.Distance(participant.transform.position, wander1.position) < 30);
		if (dance) {
			Animator anim = participant.GetComponent<Animator> ();
			//anim.SetBool("B_Breakdance", true);
			Debug.Log (participant.name + "is close enough ");

		}
        Func<bool> closeToDancer = () => (
          dance
        );
        Debug.Log(dance);
        Val<string> breakdance = Val.V (() => "breakdance");
        Val<long> duration = 1000;
        Node trigger = new LeafAssert(closeToDancer);
        Node dancing = new Sequence(participant.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(breakdance, duration));
        Node returnNode = new Sequence(trigger, dancing);
        return returnNode;
        
        //return dancing;
    }

    protected Node BuildTreeRoot()
	{
		Val<float> pp = Val.V (() => police.transform.position.z);
		Func<bool> act = () => (police.transform.position.z > 10);
        Node roaming = new Sequence(
                this.ST_ApproachAndWait(this.wander1),
                this.ST_BreakDance());
        Node root = new DecoratorLoop (roaming);
		return root;
	}
}
