using System.Collections;
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

	GameModelManager game;
	bool isReloading;
	private void Start()
	{
		game = GetComponent<Player>().game;
		canShoot = true;

		game.AddMouseEvent(InputEventType.Continious, 0, shoot);
		game.AddSimpleInputEvent(InputEventType.OnBegin, KeyCode.R,reload);

		//Bullet Factory
		Bullet.Factory = () => 
			{ return Instantiate(
				bulletPrefab,
				Vector3.zero,
				Quaternion.identity,
				game.BulletParent.transform);
			};

		//Bullet Pool
		game.PlayerBulletPool = new Pool<GameObject>(
			50,
			Bullet.Factory,
			Bullet.InitializeBullet,
			Bullet.DeactivateBullet,
			true );

		//Método de reemplazo para Destroy()
		Bullet.OnDeactivate = game.PlayerBulletPool.ReturnObjectToPool;
		game.UpdateBullets(bulletCount,maxBullets);
	}

	// Update is called once per frame
	void Update () {
		SmoothShoot();
	}

	private void reload()
	{
		if (!isReloading) StartCoroutine(reloading());
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
		if (canShoot && bulletCount > 0 && !isReloading)
		{
			anim.SetTrigger("Shoot");
			bulletCount--;
			canShoot = false;
			GameModelManager.instance.UpdateBullets(bulletCount,maxBullets);
			var newBullet = GameModelManager.instance.PlayerBulletPool.GetObjectFromPool();
			if (newBullet) newBullet.GetComponent<Bullet>()
					.Fly(cañon.transform.position, cañon.transform.forward);
		}
		else if (canShoot && bulletCount <= 0)
			reload();
	}

	IEnumerator reloading()
	{
		isReloading = true;
		anim.SetTrigger("Reload");
		canShoot = false;
		yield return new WaitForSeconds(1.4f);
		bulletCount = maxBullets;
		canShoot = true;
		isReloading = false;
		game.UpdateBullets(bulletCount, maxBullets);
	}
}
