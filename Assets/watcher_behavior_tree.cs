using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class watcher_behavior_tree : MonoBehaviour {
    public Transform wander1;
    public GameObject participant;
    GameObject[] dancers;

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

    protected Node ST_ApproachAndLook(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position,new Val<float>(5)), participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
    }

	protected Node ST_ThrowBallAt(Transform target) {
		//this should animate a ball being thrown at the dancer's transform
		Val<Vector3> position = Val.V(() => target.position);
		return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position,new Val<float>(5)), participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
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
