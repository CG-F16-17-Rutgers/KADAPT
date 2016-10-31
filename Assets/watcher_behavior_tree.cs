using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class watcher_behavior_tree : MonoBehaviour {
    public Transform wander1;
    public GameObject participant;
    GameObject[] dancers;
	private float time = 60f;
    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
		
        dancers = GameObject.FindGameObjectsWithTag("dancer");
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0)) {
			behaviorAgent.StopBehavior();
			behaviorAgent = new BehaviorAgent(this.BuildTreeRoot2());
			BehaviorManager.Instance.Register(behaviorAgent);
			behaviorAgent.StartBehavior();
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
			Debug.Log ("timer for watchers expired");
		}
	}

    protected Node ST_ApproachAndLook(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position,new Val<float>(5)), participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
    }

	protected Node ST_ThrowBallAt(Transform target) {
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
        Node roaming = new DecoratorLoop(
            new Sequence(
                this.ST_ApproachAndLook(this.wander1)
                 ));
        Node root = new DecoratorLoop(roaming); 
        return root;
    }

	protected Node BuildTreeRoot2()
	{
		Node roaming = new DecoratorLoop(
			new Sequence(
				this.ST_ThrowBallAt(this.wander1)
			));
		Node root = new DecoratorLoop(roaming); 
		return root;
	}
}
