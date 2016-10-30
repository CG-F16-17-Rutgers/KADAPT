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
        foreach (GameObject dancer in dancers)
        {
            dance = (Vector3.Distance(dancer.transform.position, participant.transform.position) < 1) && dancer != participant;
            if (dance)
            {
                Animator anim = participant.GetComponent<Animator>();
                anim.SetBool("B_Breakdance",true);
                Debug.Log(participant.name + "is dancing with " + dancer.name);
                break;
            }
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
        foreach (GameObject dancer in dancers)
        {
            dance = Vector3.Distance(dancer.transform.position, participant.transform.position) < 1.5 && dancer != participant;
            if (dance) {
                Debug.Log(participant.name);
                break; }
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
            new SequenceShuffle(
                this.ST_ApproachAndWait(this.wander1),
                this.ST_ApproachAndWait(this.wander2),
                this.ST_ApproachAndWait(this.wander3),
                this.ST_ApproachAndWait(this.wander4),
                this.ST_ApproachAndWait(this.wander5),
                this.ST_ApproachAndWait(this.wander6),
                this.ST_ApproachAndWait(this.wander7)));
        Node root = new DecoratorLoop ( new Selector(ST_BreakDance(), roaming)); 
		return root;
	}
}
