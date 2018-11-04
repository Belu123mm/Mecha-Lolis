using System.Collections.Generic;
using UnityEngine;
using Utility.Timers;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputController))]
public class Player : MonoBehaviour, IDamageable {
	[HideInInspector]
	public GameModelManager game;
	public Transform firstPersonContainer;
	public Animator CameraAnims; //Quizas esto ponerlo en otro componente.
	public Animator anim; //Mismo que el de arriba.
	//public PostProcessingProfile myProfile; //Esto quizás vuele.
	public Color NormalVignette; //Mismo de arriba.
	public Color DamageVignette; //Mismo de arriba.
	public float VignetteTime; //Mismo de arriba.

	[HideInInspector]
	public InputController controller;

	[Header("Player Stats")]
	public int Life;
	public int MaxLife;
	public CountDownTimer DamageShowOff;

	public float BaseMovementSpeed;
	public float RunningSpeed;
	public float HorizontalSpeed;
	public float VerticalSpeed;
	public bool isRunning = false;

	[Header("Default Equipment")]
	public Gun MainGun;

	float _currentMovementSpeed;
	float _vignetteEffectMax;
	float rotation = 0;
	int Jumps = 1;
	Rigidbody rb;
	IWeapon _currentWeapon;

	private void Awake()
	{
		game = FindObjectOfType<GameModelManager>();
		controller = GetComponent<InputController>();
		rb = GetComponent<Rigidbody>();
		game.PlayerList.Add(gameObject);
		_currentWeapon = MainGun;
	}

	private void Start()
	{
		//Suscribo mis eventos.
		controller.Bind(Input.GetButton, Input.GetAxis, "Horizontal", "Vertical", MoveInAxeses);
		controller.Bind(Input.GetButtonUp, Input.GetAxis,"Horizontal","Vertical", Quiet);

		controller.Bind(Input.GetButtonDown, "Run", running);
		controller.Bind(Input.GetButtonUp, "Run", running);
		controller.Bind(Input.GetButtonUp, "Jump", Jump);

		controller.Bind(Input.GetAxis, "Mouse X", "Mouse Y", RotateCamera);
		controller.Bind(Input.GetButtonDown, "Cancel", game.OpenClosePauseMenu);

		controller.Bind(Input.GetButton, "Shoot", Shoot);
		controller.Bind(Input.GetButtonDown, "Reload", Reload);

		//_vignetteEffectMax = myProfile.vignette.settings.intensity;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void MoveInAxeses(float horizontal, float vertical)
	{
        if (!anim.GetBool("IsWalking"))
            anim.SetBool("IsWalking", true);
		//Movement Speed.
		_currentMovementSpeed = isRunning ? RunningSpeed : BaseMovementSpeed;


		//Fix movimiento en Diagonal.
		var movement = (transform.right * horizontal) + (transform.forward * vertical);
		transform.position +=  movement * _currentMovementSpeed * Time.deltaTime;
	}
	public void Quiet(float Horizontal, float Vertical)
	{
        //print("Horizontal is :" + Horizontal + " and Vertical is: " + Vertical + ".");
        if ((int)Horizontal == 0 && (int)Vertical == 0) anim.SetBool("IsWalking", false);
	}

	public void RotateCamera(float x, float y)
	{
		//Roto el personaje en horizontal.
		transform.Rotate(0, HorizontalSpeed * x, 0);

		//Roto la cámara del personaje en vertical
		rotation += VerticalSpeed * y;
		rotation = Mathf.Clamp(rotation, -40, 90);
		var myNewRot = new Vector3( rotation, 0, firstPersonContainer.transform.localEulerAngles.z);
		firstPersonContainer.localEulerAngles = -myNewRot;
	}

	public void running()
	{
		isRunning = !isRunning;
		print(isRunning ? "Estoy corriendo" : "No estoy corriendo");
	}

	public void Jump()
	{
		if (Jumps == 1)
		{
			rb.AddForce(transform.up * 400);
			Jumps--;
		}
	}

	public void AddDamage(int Damage)
	{
		CameraAnims.SetTrigger("CameraShake");
		Life -= Damage;
		//StartCoroutine(ShowDamage());

		if (Life <= 0)
		{
			print("Estas muerto");
			game.EndGame();
		}
		game.UpdateLife((float)Life/MaxLife);
		print("Recibiste daño!");
	}

	public void SwitchToMainWeapon()
	{
		_currentWeapon = MainGun;
		_currentWeapon.OnSelect();
	}
	public void Shoot()
	{
		_currentWeapon.Shoot();
	}
	public void Reload()
	{
		_currentWeapon.Reload();
	}

	//IEnumerator ShowDamage()
	//{
	//	myProfile.vignette.enabled = true;
	//	myProfile.chromaticAberration.enabled = true;
	//	myProfile.grain.enabled = true;
	//	var sets = myProfile.vignette.settings;
	//	sets.color = DamageVignette;
	//	while (sets.intensity >= 0)
	//	{
	//		sets.intensity -= 0.008f;
	//		myProfile.vignette.settings = sets;
	//		yield return new WaitForEndOfFrame();
	//	}

	//	myProfile.vignette.enabled = false;
	//	myProfile.chromaticAberration.enabled = false;
	//	myProfile.grain.enabled = false;
	//	sets.intensity = _vignetteEffectMax;
	//	sets.color = NormalVignette;
	//	myProfile.vignette.settings = sets;
	//}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 8) Jumps = 1;
	}
}
