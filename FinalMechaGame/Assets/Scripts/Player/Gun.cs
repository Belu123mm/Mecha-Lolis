using System;
using System.Collections;
using UnityEngine;
using Utility.Timers;

[Serializable]
public class Gun : MonoBehaviour, IWeapon {
	//public TPGranade granadeHability;
	public Animator anim;
	public GameObject bulletPrefab;
	public GameObject cañon;

    CountDownTimer gunRecoil;
	GameModelManager game;

    [Header("Gun Stats")]
	public int bulletCount;
	public int maxBullets;
	public float recoil;

	bool isReloading;
	private void Start()
	{
		game = GetComponent<Player>().game;
        gunRecoil = new CountDownTimer(gameObject).SetCoolDown(recoil);

        var controller = GetComponent<Player>().controller;
        controller.Bind(Input.GetButton, "Shoot", Shoot);
        controller.Bind(Input.GetButton, "Reload", reload);

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

	private void reload()
	{
		if (!isReloading) StartCoroutine(reloading());
	}

	public void Shoot()
	{
		if (gunRecoil.Ready && bulletCount > 0 && !isReloading)
		{
			anim.SetTrigger("Shoot");
			bulletCount--;
            gunRecoil.StartCount();
			GameModelManager.instance.UpdateBullets(bulletCount,maxBullets);
			var newBullet = GameModelManager.instance.PlayerBulletPool.GetObjectFromPool();
			if (newBullet) newBullet.GetComponent<Bullet>()
					.Fly(cañon.transform.position, cañon.transform.forward);
		}
		else if (gunRecoil.Ready && bulletCount <= 0)
			reload();
        if (bulletCount < (maxBullets * 0.3))
            game.AdviceLowAmmo(true);

        if (bulletCount == 0)
            game.AdviceReload(true);
	}

	IEnumerator reloading()
	{
		isReloading = true;
		anim.SetTrigger("Reload");
		yield return new WaitForSeconds(1.4f);
		bulletCount = maxBullets;
        gunRecoil.Reset();
        isReloading = false;
		game.UpdateBullets(bulletCount, maxBullets);
        game.AdviceLowAmmo(false);
        game.AdviceReload(false);
	}

    public void OnSelect()
    {
        throw new System.NotImplementedException("No implementaste esta función todavía");
    }
}
