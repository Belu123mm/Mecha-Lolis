using UnityEngine;

public class Turret : MonoBehaviour, IDamageable {
	public BulletGroup bulletgroup;
	public Transform target;
	public AreaController control;

	[Header("Stats")]
	public float timetoshoot;
	public int Life;

	void Start () {
		ModelTurret _m = new ModelTurret(target, timetoshoot, bulletgroup);
		control = new AreaController(_m);
	}

	void Update () {
		control.OnUpdate();
		control.GetTarget(target);
	}

	//public void OnCollisionEnter( Collision collision ) {
	//    if ( collision.gameObject.layer == LayerMask.NameToLayer("MyBulletz") )
	//        Destroy(gameObject);
	//}
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
