using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class dancer_behavior_tree : MonoBehaviour
{
    public Transform wander1;
    public GameObject participant;
	private BehaviorAgent behaviorAgent;
	private float time = 30f;
    // Use this for initialization
    void Start ()
	{
		
		wander1.position = new Vector3(UnityEngine.Random.Range(10, 20), 0, UnityEngine.Random.Range(-10, 20)); 
        behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		timerStart ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0)) {
			behaviorAgent.StopBehavior();
		}
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

    protected Node ST_BreakDance()
    {
        bool dance = false;
        dance = (Vector3.Distance(participant.transform.position, wander1.position) < 30);
		if (dance) {
			Animator anim = participant.GetComponent<Animator> ();
		}
        Func<bool> closeToDancer = () => (
          dance
        );
        Val<string> breakdance = Val.V (() => "breakdance");
        Val<long> duration = 1000;
        Node trigger = new LeafAssert(closeToDancer);
        Node dancing = new Sequence(participant.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(breakdance, duration));
        Node returnNode = new Sequence(trigger, dancing);
        return returnNode;
    }

    protected Node BuildTreeRoot()
	{
        Node roaming = new Sequence(
                this.ST_ApproachAndWait(this.wander1),
                this.ST_BreakDance());
        Node root = new DecoratorLoop (roaming);
		return root;
	}
}
