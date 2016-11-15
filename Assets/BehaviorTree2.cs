using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class BehaviorTree2 : MonoBehaviour
{

	public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public GameObject participant;
    public GameObject talkTarget;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
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
        Val<float> radius = Val.V(() => 3f);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position, radius),
            new LeafWait(1000));
    }

    protected Node ST_PhoneCall()
    {
        Val<string> phone = Val.V(() => "Talking On Phone");
        Val<bool> start = Val.V(() => true);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation(phone,start));
            
    }

    protected Node ST_Face(Vector3 target)
    {
        Val<Vector3> targetPosition = Val.V(() => target);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(targetPosition));
    }
    protected Node BuildTreeRoot()
    {
        Node roaming =
                        new Sequence(
                        this.ST_ApproachAndWait(this.wander3),
                        this.ST_ApproachAndWait(this.wander1)
                        );
        return roaming;
    }
}
