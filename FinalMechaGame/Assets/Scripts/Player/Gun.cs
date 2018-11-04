using System;
using System.Collections;
using UnityEngine;
using Utility.Timers;

[Serializable]
public class Gun : MonoBehaviour, IWeapon {
	public Animator anim;
	public GameObject cañon;
	public CountDownTimer gunRecoil;

	[Header("Gun Stats")]
	public int bulletCount;
	public int maxBullets;
	public float recoil;
	[Tooltip("WhenEver this Gun can reload when BulletCount is 0(Empty).")]
	public bool CanReload;

	bool isReloading = false;

    //------------------------------Monobehaviour Methods---------------------------------------
    private void Awake()
    {
        //gunRecoil = new CountDownTimer(gameObject, 0.1f, 0.01f);
    }
    //-------------------------------------Methods----------------------------------------------

    public void OnSelect()
	{
		print("La ametralladora ha sido seleccionada.");
		
	}
	public void Shoot()
	{
        if (isReloading) return;

		if (bulletCount <= 0)
		{
			Reload();
			return;
		}
        else if (gunRecoil.isReady)
		{

            print(isReloading);

			anim.SetTrigger("Shoot");
			bulletCount--;

			GameModelManager.instance.UpdateBullets(bulletCount,maxBullets);
			var newBullet = GameModelManager.instance.PlayerBulletPool.GetObjectFromPool();
			if (newBullet) newBullet.GetComponent<Bullet>()
				.Fly(cañon.transform.position, cañon.transform.forward);

			gunRecoil.StartCount();
		}

		if (bulletCount < (maxBullets * 0.3)) GameModelManager.instance.AdviceLowAmmo(true);
		if (bulletCount == 0) GameModelManager.instance.AdviceReload(true);
	}
	public void Reload()
	{
		if (!isReloading && CanReload && bulletCount < maxBullets)
        {
            isReloading = true;
            anim.SetTrigger("Reload");
            StartCoroutine(reloading());
        }

	}
	IEnumerator reloading()
	{
		yield return new WaitForSeconds(1.4f);

		bulletCount = maxBullets;
		isReloading = false;
		GameModelManager.instance.UpdateBullets(bulletCount, maxBullets);
		GameModelManager.instance.AdviceLowAmmo(false);
		GameModelManager.instance.AdviceReload(false);
	}
}
