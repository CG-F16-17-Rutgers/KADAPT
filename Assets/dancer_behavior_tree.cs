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
    // Use this for initialization
    void Start ()
	{
        dancers = GameObject.FindGameObjectsWithTag("dancer");
        behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}

	// Update is called once per frame
	void Update ()
	{
        bool dance = false;
        
            dance = (Vector3.Distance(participant.transform.position, wander1.position) < 2f);
            if (dance)
            {
                Animator anim = participant.GetComponent<Animator>();
                anim.SetBool("B_Breakdance",true);
                //Debug.Log(participant.name + "is dancing with " + dancer.name);
                
            }
    }

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position); 
        return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

    protected Node ST_Watch(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(10000));
    }
    
    protected Node ST_BreakDance()
    {
        GameObject[] dancers = GameObject.FindGameObjectsWithTag("dancer");
        bool dance = false;

        dance = (Vector3.Distance(participant.transform.position, wander1.position) < 1);
        if (dance)
        {
            Animator anim = participant.GetComponent<Animator>();
            anim.SetBool("B_Breakdance", true);
            //Debug.Log(participant.name + "is dancing with " + dancer.name);

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
        Node roaming = new DecoratorLoop(
            new Sequence(
                this.ST_ApproachAndWait(this.wander2),
                this.ST_ApproachAndWait(this.wander1)));
        Node root = new DecoratorLoop ( new Sequence(roaming,this.ST_BreakDance())); 
		return root;
	}
}
