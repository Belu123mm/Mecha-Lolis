using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {
	public float force;
	public float timeToDie = 3;
	public int Damage;
	public int DamageableLayer;

	void Update () {
		//transform.position += transform.forward * force * Time.deltaTime;
		GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);

		timeToDie -= Time.deltaTime;
		if (timeToDie <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == DamageableLayer)
		{
			other.gameObject.GetComponentInParent<IDamageable>().AddDamage(Damage);
			Destroy(gameObject);
		}
	}
}
