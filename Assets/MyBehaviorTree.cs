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
    public GameObject rightHand;
    public Transform rightHandPosition;
    public GameObject swordText;
    public GameObject treasureText;
    public GameObject monster;
    public Transform eat;
    public Transform watch;
    public Transform pickup;
    public Transform eatPosition;
    public Transform end;

    // Use this for initialization
    void Start ()
	{
        swordText.GetComponent<TextMesh>().text = "";
		swordPosition.position = new Vector3(UnityEngine.Random.Range(-20, 30), 0.5f, UnityEngine.Random.Range(-20, 20));
		sword.transform.position = swordPosition.position;
        treasureText.GetComponent<TextMesh>().text = "";
        behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	void SetText(GameObject obj, string s)
    {
        obj.GetComponent<TextMesh>().text = s;
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
        if (!obj.Equals(monster)) { obj.GetComponent<UnitySteeringController>().maxSpeed = 5f; }
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
             talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "pickupright")), Val.V(() => 1000L)),
            new LeafInvoke(() => swordInHand())
            );
        
    }
    void swordInHand()
    {
        sword.transform.parent = rightHandPosition.transform;
        sword.transform.rotation = rightHandPosition.rotation;
        sword.transform.position = rightHandPosition.transform.position;
    }
    void pickupAndKill()
    {
        wanderer2.transform.parent = eatPosition.transform;
        wanderer2.transform.rotation = eatPosition.rotation;
        wanderer2.transform.position = eatPosition.transform.position;
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
    protected Node ST_wander2Reveal(GameObject killer, GameObject victim)
    {

        return new Sequence(
            new SequenceParallel(
                killer.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => victim.transform.position)),
                 victim.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => killer.transform.position))
               ),
            victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "stayaway")), Val.V(() => 500L)),
           
            victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "stepback")), Val.V(() => 1000L)),
            new LeafInvoke(() => SetText(treasureText, "Please don't hurt me!\n I'll show you where the treasure lies..."))
            //killer.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "jab")), Val.V(() => 3000L))
            //victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "dying")), Val.V(() => 1000L))
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
                                new LeafInvoke(() => SetText(swordText, "It's dangerous out there!\n Take a sword...")),
                                talkTarget.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => participant.transform.position)),
                                new LeafWait(3000),
                                new LeafInvoke(() => SetText(swordText, "")),
                                this.ST_TakeSword(), new LeafWait(1000),

                                new SequenceParallel(this.ST_ApproachAndWait(talkTarget, this.desertStation),
                                        this.ST_ApproachAndWait(wanderer1, this.surround1),
                                           this.ST_ApproachAndWait(wanderer2, this.surround2),
                                           this.ST_ApproachAndWait(wanderer3, this.surround3)

                                    ),
                                    this.ST_kill(talkTarget, wanderer1),
                                    this.ST_kill(talkTarget, wanderer3),
                                    this.ST_wander2Reveal(talkTarget, wanderer2),
                                    new LeafWait(3000),
                                    new LeafInvoke(() => SetText(treasureText, "")),
                                    new SequenceParallel(
                                        this.ST_ApproachAndWait(wanderer2, eat),
                                        this.ST_ApproachAndWait(talkTarget, watch)
                                    ),
                                    new LeafWait(1000),
                                    this.ST_ApproachAndWait(talkTarget, pickup),
                                    new LeafWait(3000),
                                     monster.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => talkTarget.transform.position)),
                                     new LeafWait(500),
                                     monster.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "roar")), Val.V(() => 1000L)),
                                     talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "stepback")), Val.V(() => 1000L)),
                                     this.ST_ApproachAndWait(monster, pickup.transform),
                                     monster.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => talkTarget.transform.position)),
                                     talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "fight")), Val.V(() => 1000L)),
                                     new SequenceParallel(
                                         new Sequence(
                                             talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "duck")), Val.V(() => 1000L)),
                                             talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "duck")), Val.V(() => 1000L))

                                         ),
                                     monster.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "jab")), Val.V(() => 1000L)),
                                     monster.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "roar")), Val.V(() => 1000L))
                                    ),
                                     new LeafWait(1000),
                                     talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "jab")), Val.V(() => 1000L)),
                                     talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "jab")), Val.V(() => 1000L)),
                                      monster.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "dying")), Val.V(() => 1000L)),
                                      new LeafWait(1000),
                                      this.ST_ApproachAndWait(talkTarget,end),
                                      talkTarget.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "breakdance")), Val.V(() => 4000L))




                                )
                                
                        );
		return roaming;
	}
}
