using System;
using UnityEngine;

public class Bullet : MonoBehaviour{
	public static Func<GameObject> Factory;
	public static Action<GameObject> OnDeactivate;

	public int Damage;
	public int DamageableLayer;
	public int WorldLayer;
	public float force;
	public float timeToDie = 3f;

	public static void InitializeBullet(GameObject bulletObj)
	{
		bulletObj.gameObject.SetActive(true);
	}
	public static void DeactivateBullet(GameObject bulletObj)
	{
		bulletObj.gameObject.SetActive(false);
		bulletObj.GetComponent<TrailRenderer>().Clear();
		bulletObj.transform.position = Vector3.zero;
		bulletObj.transform.rotation = Quaternion.identity;
	}

	void Update () {

		timeToDie -= Time.deltaTime;
		if (timeToDie <= 0)
		{
			OnDeactivate(gameObject);
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			timeToDie = 3f;
		}
	}

	public void Fly(Vector3 from, Vector3 to)
	{
		transform.position = from;
		transform.forward = to;
		GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == WorldLayer)
			OnDeactivate(gameObject);
		if (other.gameObject.layer == DamageableLayer)
		{
			other.gameObject.GetComponentInParent<IDamageable>().AddDamage(Damage);
			OnDeactivate(gameObject);
		}
	}
}
