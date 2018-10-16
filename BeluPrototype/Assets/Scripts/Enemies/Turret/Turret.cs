using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public Transform target;
    public Sight sight;
    public NodeGroup nodegroup;


    [Header("Stats")]
    public float timetoshoot;
    public int Life;


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
