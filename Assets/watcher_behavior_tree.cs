using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class watcher_behavior_tree : MonoBehaviour {
    public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public Transform wander4;
    public Transform wander5;
    public Transform wander6;
    public Transform wander7;
    public GameObject participant;
    public GameObject police;
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
       
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }
    protected Node ST_Watch()
    {
        int duration = 0;
        bool watch = false; ;
        Val<Vector3> position = null;
        foreach ( GameObject dancer in dancers)
        {
            Animator anim = dancer.GetComponent<Animator>();
            bool isDancing = anim.GetBool("B_Breakdance");
            Debug.Log(isDancing);
            watch = (Vector3.Distance(dancer.transform.position, participant.transform.position) < 5) && isDancing;
            if (watch)
            {
                Debug.Log(participant.name + " is watching " + dancer.name);
                position = Val.V(() => dancer.transform.position);
                duration = 1000;
                break;
            }
        }
        if (position == null)
        {
            position = participant.transform.forward;
        }
        Func<bool> watching = () => (
          watch
        );
        Node trigger = new LeafAssert(watching);
        return new Sequence(trigger, participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),new LeafWait(duration));
    }
    protected Node BuildTreeRoot()
    {
        Val<float> pp = Val.V(() => police.transform.position.z);
        Func<bool> act = () => (police.transform.position.z > 10);
        Node roaming = new DecoratorLoop(
            new Sequence(
                this.ST_ApproachAndWait(this.wander1),
                 this.ST_ApproachAndWait(this.wander2),
                 this.ST_ApproachAndWait(this.wander3),
                 this.ST_ApproachAndWait(this.wander4),
                 this.ST_ApproachAndWait(this.wander5),
                 this.ST_ApproachAndWait(this.wander6),
                 this.ST_ApproachAndWait(this.wander7)));

        Node trigger = new DecoratorLoop(new LeafAssert(act));
        Node root = new DecoratorLoop(new Selector(this.ST_Watch(),roaming)); 
        return root;
    }
}
