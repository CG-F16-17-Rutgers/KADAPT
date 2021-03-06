﻿using UnityEngine;
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
	public Transform wander11;

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
	private float time = 300f;
    private BehaviorAgent behaviorAgent;
    public InteractionSystem int1;
    GameObject killer;
	private int totalGone;
	private int totalDone;
	private float nearSafePlaceX1;
	private float nearSafePlaceX2;
	private float nearSafePlaceZ1;
	private float nearSafePlaceZ2;
 
    // Use this for initialization
    void Start ()
	{	
		nearSafePlaceX1 = wander11.position.x + 3;
		nearSafePlaceX2 = wander11.position.x - 3;
		nearSafePlaceZ1 = wander11.position.z + 3;
		nearSafePlaceZ2 = wander11.position.z - 3;
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
		totalDone = hitDetector.total + totalGone;
		Debug.Log (totalDone);
		if (time == 0 || totalDone == 20) {
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
		var NPCS = GameObject.FindGameObjectsWithTag ("NPC");
		var NPCCount = NPCS.Length;
		foreach (var NPC in NPCS) {
			if (NPC.transform.position.x < nearSafePlaceX1 && NPC.transform.position.x > nearSafePlaceX2 && NPC.transform.position.z < nearSafePlaceZ1 && NPC.transform.position.z > nearSafePlaceZ2) {
				Debug.Log ("in safe place");
				removeObj (NPC);
			}
		}

	}
    
	//this should be called when the participants reach wander3
	void removeObj(GameObject obj) {
		totalGone++;
		GameObject.DestroyObject (obj);
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
                        new Selector(
                            new DecoratorLoop(
                            new SequenceParallel(
                            new DecoratorLoop((new DecoratorInvert(new LeafAssert(() => hitDetector.panic)))),
                            //these are the activities before the killer is spotted
                            new DecoratorLoop(new SequenceParallel( 
                             ST_ApproachAndWait(participant1, wander1),
                             ST_ApproachAndWait(participant2, wander1),
                             ST_ApproachAndWait(participant3, wander2),
                             ST_ApproachAndWait(participant4, wander2),
                             ST_ApproachAndWait(participant5, wander3),
                             ST_ApproachAndWait(participant6, wander3),
                             ST_ApproachAndWait(participant7, wander4),
                             ST_ApproachAndWait(participant8, wander4),
                             ST_ApproachAndWait(participant9, wander5),
                             ST_ApproachAndWait(participant10, wander5),
                             ST_ApproachAndWait(participant11, wander6),
                             ST_ApproachAndWait(participant12, wander6),
                             ST_ApproachAndWait(participant13, wander7),
                             ST_ApproachAndWait(participant14, wander7),
                             ST_ApproachAndWait(participant15, wander8),
                             ST_ApproachAndWait(participant16, wander8),
                             ST_ApproachAndWait(participant17, wander9),
                             ST_ApproachAndWait(participant18, wander9),
                             ST_ApproachAndWait(participant19, wander10),
                             ST_ApproachAndWait(participant20, wander10)
                             )))),
                            new DecoratorLoop(new SequenceParallel(
                                //these are the activites after the killer is spotted
                             ST_ApproachAndWait(participant1, wander11),
                             ST_ApproachAndWait(participant2, wander11),
                             ST_ApproachAndWait(participant3, wander11),
                             ST_ApproachAndWait(participant4, wander11),
                             ST_ApproachAndWait(participant5, wander11),
                             ST_ApproachAndWait(participant6, wander11),
                             ST_ApproachAndWait(participant7, wander11),
                             ST_ApproachAndWait(participant8, wander11),
                             ST_ApproachAndWait(participant9, wander11),
                             ST_ApproachAndWait(participant10, wander11),
                             ST_ApproachAndWait(participant11, wander11),
                             ST_ApproachAndWait(participant12, wander11),
                             ST_ApproachAndWait(participant13, wander11),
                             ST_ApproachAndWait(participant14, wander11),
                             ST_ApproachAndWait(participant15, wander11),
                             ST_ApproachAndWait(participant16, wander11),
                             ST_ApproachAndWait(participant17, wander11),
                             ST_ApproachAndWait(participant18, wander11),
                             ST_ApproachAndWait(participant19, wander11),
                             ST_ApproachAndWait(participant20, wander11)
                                ))
                            )
                        
                        );
		return roaming;
	}
}
