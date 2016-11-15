using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
	public GameObject participant;
    public GameObject talkTarget;
    public InteractionObject sword;
    public Transform swordPosition;
	private BehaviorAgent behaviorAgent;
    public InteractionSystem int1;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected Node ST_ApproachAndWait(GameObject obj, Transform target)
	{
        Val<Vector3> position = Val.V (() => target.position);
        Val<float> radius = Val.V(() => 3f);
        return new Sequence(obj.GetComponent<BehaviorMecanim>().Node_GoTo(position),
            new LeafWait(1000));
	}
    protected Node ST_TakeSword()
    {
        return new Sequence(
            talkTarget.GetComponent<BehaviorMecanim>().ST_TurnToFace(sword.transform.position),
            new LeafWait(500),
            ST_ApproachAndWait(talkTarget, swordPosition),
            new LeafInvoke(() => talkTarget.GetComponent<InteractionSystem>().StartInteraction(FullBodyBipedEffector.RightHand,sword,true)
            )
            );
    }
	protected Node BuildTreeRoot()
	{
        Node roaming =
                        new SequenceParallel(
                            new Sequence(
                                this.ST_ApproachAndWait(participant, this.wander2), new LeafWait(3000),
                                participant.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() =>talkTarget.transform.position))
                            ),
                            new Sequence(
                                this.ST_ApproachAndWait(talkTarget, this.wander1),
                                talkTarget.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => participant.transform.position)),
                                this.ST_TakeSword())
                                
                        );
		return roaming;
	}
}
