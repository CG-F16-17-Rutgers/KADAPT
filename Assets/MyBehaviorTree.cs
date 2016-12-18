using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;
using UnityEngine.SceneManagement;

public class MyBehaviorTree : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
    public Transform wander4;
    public Transform wander5;
    public Transform wander6;
    public Transform wander7;
    public Transform wander8;
    public Transform wander9;
    public Transform wander10;

    public GameObject participant1;
    public GameObject participant2;
    public GameObject participant3;
    public GameObject participant4;
    public GameObject participant5;
    public GameObject participant6;
    public GameObject participant7;
    public GameObject participant8;
    public GameObject participant9;
    public GameObject participant10;
    public GameObject participant11;
    public GameObject participant12;
    public GameObject participant13;
    public GameObject participant14;
    public GameObject participant15;
    public GameObject participant16;
    public GameObject participant17;
    public GameObject participant18;
    public GameObject participant19;
    public GameObject participant20;
    
	public TextMesh Timer;
	private float time = 120f;
    private BehaviorAgent behaviorAgent;
    public InteractionSystem int1;
    GameObject killer;
    

 
    // Use this for initialization
    void Start ()
	{

        behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		Timer.text = "Time Left: " + time;
		timerStart ();
	}
	void SetText(GameObject obj, string s)
    {
        obj.GetComponent<TextMesh>().text = s;
    }

	public void timerStart() {
		InvokeRepeating ("Countdown", 1.0f, 1.0f);
	}

	void Countdown () {
		time--;
		if (time == 0) {
			CancelInvoke ("Countdown");
			saveScore ();
			SceneManager.LoadScene ("B5end");
		}
		Timer.text = "Time Left: " + time;

	}

	void saveScore() {
		PlayerPrefs.SetInt("Kills", hitDetector.total);
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
         obj.GetComponent<UnitySteeringController>().maxSpeed = 30f; 
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
                        new Selector(
                            new DecoratorLoop(
                            new SequenceParallel(
                            new DecoratorLoop((new DecoratorInvert(new LeafAssert(() => hitDetector.panic)))),
                            //these are the activities before the killer is spotted
                            new DecoratorLoop(new SequenceParallel( 
                             ST_ApproachAndWait(participant1, wander3),
                             ST_ApproachAndWait(participant2, wander8),
                             ST_ApproachAndWait(participant3, wander5),
                             ST_ApproachAndWait(participant4, wander2),
                             ST_ApproachAndWait(participant5, wander3),
                             ST_ApproachAndWait(participant6, wander3),
                             ST_ApproachAndWait(participant7, wander4),
                             ST_ApproachAndWait(participant8, wander5),
                             ST_ApproachAndWait(participant9, wander6),
                             ST_ApproachAndWait(participant10, wander7),
                             ST_ApproachAndWait(participant11, wander1),
                             ST_ApproachAndWait(participant12, wander2),
                             ST_ApproachAndWait(participant13, wander3),
                             ST_ApproachAndWait(participant14, wander4),
                             ST_ApproachAndWait(participant15, wander5),
                             ST_ApproachAndWait(participant16, wander6),
                             ST_ApproachAndWait(participant17, wander7),
                             ST_ApproachAndWait(participant18, wander8),
                             ST_ApproachAndWait(participant19, wander9),
                             ST_ApproachAndWait(participant20, wander10)
                             )))),
                            new DecoratorLoop(new SequenceParallel(
                                //these are the activites after the killer is spotted
                                ST_ApproachAndWait(participant1, wander3),
                             ST_ApproachAndWait(participant2, wander3),
                             ST_ApproachAndWait(participant3, wander3),
                             ST_ApproachAndWait(participant4, wander3),
                             ST_ApproachAndWait(participant5, wander3),
                             ST_ApproachAndWait(participant6, wander3),
                             ST_ApproachAndWait(participant7, wander3),
                             ST_ApproachAndWait(participant8, wander3),
                             ST_ApproachAndWait(participant9, wander3),
                             ST_ApproachAndWait(participant10, wander3),
                             ST_ApproachAndWait(participant11, wander3),
                             ST_ApproachAndWait(participant12, wander3),
                             ST_ApproachAndWait(participant13, wander3),
                             ST_ApproachAndWait(participant14, wander3),
                             ST_ApproachAndWait(participant15, wander3),
                             ST_ApproachAndWait(participant16, wander3),
                             ST_ApproachAndWait(participant17, wander3),
                             ST_ApproachAndWait(participant18, wander3),
                             ST_ApproachAndWait(participant19, wander3),
                             ST_ApproachAndWait(participant20, wander3)
                                ))
                            )
                        
                        );
		return roaming;
	}
}
