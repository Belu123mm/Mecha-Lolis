using System.Collections;
using System.Collections.Generic;
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
			navigation.rigidbody = GetComponent<Rigidbody>();
		}
		sight = GetComponent<Sight>();
		sight.targetTransform = target;
	}

	public void AddDamage(int Damage)
	{
		Life -= Damage;
		if (Life <= 0)
		{
			GameModelManager.instance.Points += 10;
			GameModelManager.instance.UpdatePoints();
			Destroy(gameObject);
		}
		print(name + " ha recibido " + Damage + " puntos de daño!");
	}
}
