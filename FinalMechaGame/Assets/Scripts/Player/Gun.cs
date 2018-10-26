using UnityEngine;

public class Gun : MonoBehaviour {
	//public TPGranade granadeHability;
	public Animator anim;
	public GameObject bulletPrefab;
	public GameObject cañon;
	public float recoil;
	public float cooldown;
	public int bulletCount;
	public int maxBullets;
	public bool canShoot = false;
	public bool lockShoot = false;

	private void Start()
	{
		canShoot = true;

		GameModelManager.instance.AddMouseEvent(InputEventType.Continious, 0, shoot);
		GameModelManager.instance.AddSimpleInputEvent(InputEventType.OnBegin, KeyCode.R,reload);


		//Bullet Factory
		Bullet.Factory =
			() => { return Instantiate(
                bulletPrefab,
                Vector3.zero,
                Quaternion.identity,
                GameModelManager.instance.BulletParent.transform);
            };

		//Bullet Pool
		GameModelManager.instance.PlayerBulletPool = new Pool<GameObject>(
			10,
			Bullet.Factory,
			Bullet.InitializeBullet,
			Bullet.DeactivateBullet,
			false );

		//Método de reemplazo para Destroy()
		Bullet.OnDeactivate = GameModelManager.instance.PlayerBulletPool.ReturnObjectToPool;
	}

	// Update is called once per frame
	void Update () {
		SmoothShoot();
	}

	private void reload()
	{
		print("Reloaded");
		anim.SetTrigger("Reload");
		bulletCount = maxBullets;
	}

	void SmoothShoot()
	{
		if (!canShoot)
		{
			if (recoil <= 0)
			{
				recoil = cooldown;
				canShoot = true;
				print("Puedo disparar.");
				return;
			}

			recoil -= Time.deltaTime;
		}
	}

	private void shoot()
	{
		if (canShoot && bulletCount > 0 && !lockShoot)
		{
			anim.SetTrigger("Shoot");
			bulletCount--;
			canShoot = false;
			var newBullet = GameModelManager.instance.PlayerBulletPool.GetObjectFromPool();
			if (newBullet) newBullet.GetComponent<Bullet>()
					.Fly(cañon.transform.position, cañon.transform.forward);
		}
	}
}
