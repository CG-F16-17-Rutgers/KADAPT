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
	private BehaviorAgent behaviorAgent;
    public InteractionSystem int1;
    

 
    // Use this for initialization
    void Start ()
	{

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
         obj.GetComponent<UnitySteeringController>().maxSpeed = 5f; 
        Val<Vector3> position = Val.V (() => target.position);
        Val<float> radius = Val.V(() => 3f);
        return new Sequence(obj.GetComponent<BehaviorMecanim>().Node_GoTo(position),
            new LeafWait(1000));
	}

    protected Node ST_checkIfClose(GameObject participant, GameObject killer)
    {
        return new Sequence();
    }


    protected Node ST_wander2Reveal(GameObject killer, GameObject victim)
    {

        return new Sequence(
            new SequenceParallel(
                killer.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => victim.transform.position)),
                 victim.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => killer.transform.position))
               ),
            victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "stayaway")), Val.V(() => 500L)),
           
            victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "stepback")), Val.V(() => 1000L))
            
            //killer.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "jab")), Val.V(() => 3000L))
            //victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture((Val.V(() => "dying")), Val.V(() => 1000L))
           );
    }
    protected Node BuildTreeRoot()
	{
        Node roaming =
                        new DecoratorLoop(
                        new Sequence(
                            ST_ApproachAndWait(participant, wander1),
                             ST_ApproachAndWait(participant, wander2)
                        )
                        );
		return roaming;
	}
}
