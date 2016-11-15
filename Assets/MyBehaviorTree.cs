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
    public Transform desertStation;
	public GameObject participant;
    public GameObject talkTarget;
    public InteractionObject sword;
    public Transform swordPosition;
	private BehaviorAgent behaviorAgent;
    public InteractionSystem int1;
    public GameObject wanderer1;
    public GameObject wanderer2;
    public GameObject wanderer3;
    public Transform surround1;
    public Transform surround2;
    public Transform surround3;
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
    //this method is currently acting strangely
    protected Node ST_RunAndWait(GameObject obj, Transform target)
    {
        obj.GetComponent<UnitySteeringController>().maxSpeed = 10f;
        //obj.GetComponent<NavMeshAgent>().speed = 10;
        Val<Vector3> position = Val.V(() => target.position);
        Val<float> radius = Val.V(() => 3f);
        return new Sequence(obj.GetComponent<BehaviorMecanim>().Node_GoTo(position),
            new LeafWait(1000));
    }

    protected Node ST_ApproachAndWait(GameObject obj, Transform target)
	{
        obj.GetComponent<UnitySteeringController>().maxSpeed = .6f;
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
            new LeafInvoke(() => talkTarget.GetComponent<InteractionSystem>().StartInteraction(FullBodyBipedEffector.RightHand, sword, true)
            )   
            );
        
    }

    protected Node ST_kill(GameObject killer, GameObject victim)
    {
     
        return new Sequence(
            new SequenceParallel(
                killer.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => victim.transform.position)),
                 victim.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => killer.transform.position))
                ),
            killer.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "jab")),Val.V(()=>1000L)),
            victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "dying")), Val.V(() => 1000L))
           );
    }
	protected Node BuildTreeRoot()
	{
        Node roaming =
                        new SequenceParallel(
                            new Sequence(
                                this.ST_ApproachAndWait(participant, this.wander2), new LeafWait(3000),
                                participant.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => talkTarget.transform.position))
                            ),
                            new Sequence(
                                this.ST_ApproachAndWait(talkTarget, this.wander1),
                                talkTarget.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => participant.transform.position)),
                                new LeafWait(3000),
                                this.ST_TakeSword(), new LeafWait(1000),
                                new SequenceParallel(this.ST_ApproachAndWait(talkTarget, this.desertStation),
                                        this.ST_ApproachAndWait(wanderer1, this.surround1),
                                           this.ST_ApproachAndWait(wanderer2, this.surround2),
                                           this.ST_ApproachAndWait(wanderer3, this.surround3)

                                    ),
                                    this.ST_kill(talkTarget, wanderer1),
                                    this.ST_kill(talkTarget, wanderer2),
                                    this.ST_kill(talkTarget, wanderer3)

                                )
                                
                        );
		return roaming;
	}
}
