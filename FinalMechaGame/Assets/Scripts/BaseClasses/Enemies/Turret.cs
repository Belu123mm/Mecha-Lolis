using UnityEngine;

public class Turret : MonoBehaviour {
	public bool hasNavigation;
	[HideInInspector]
	public NodeGroup nodegroup;
	[HideInInspector]
	public Transform target;
	[HideInInspector]
	public Sight sight;
	[HideInInspector]
	public NodeNavigation navigation;

	[Header("Stats")]
	public float timetoshoot = 0.001f;
	public int Life;
    public int burstQuantity = 1;
    public float timeBtBurst = 0.01f;
    public float timer;
    public string state;

	public virtual void Start()
	{
		if ( hasNavigation )
		{
			navigation = GetComponent<NodeNavigation>();
			navigation.currentNode = nodegroup._first;
			navigation.myRigidBody = GetComponent<Rigidbody>();
		}
		sight = GetComponent<Sight>();
		sight.targetTransform = target;
	}
}
