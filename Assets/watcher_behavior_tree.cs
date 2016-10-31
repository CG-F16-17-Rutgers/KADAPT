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
       
    }

    protected Node ST_ApproachAndLook(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position,new Val<float>(5)), participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
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
            watch = (Vector3.Distance(dancer.transform.position, participant.transform.position) < 5) && isDancing;
            if (watch)
            {
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
        Node roaming = new DecoratorLoop(
            new Sequence(
                this.ST_ApproachAndLook(this.wander1)
                 ));
        Node root = new DecoratorLoop(roaming); 
        return root;
    }
}
