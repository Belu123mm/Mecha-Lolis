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
	public float timetoshoot;
	public int Life;

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
