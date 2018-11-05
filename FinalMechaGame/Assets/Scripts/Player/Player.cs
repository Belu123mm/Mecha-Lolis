using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputController))]
public class Player : MonoBehaviour, IDamageable {
	[HideInInspector]
	public GameModelManager game;
	public Transform firstPersonContainer;
	public Animator CameraAnims; //Quizas esto ponerlo en otro componente.
	public Animator PlayerAnims; //Mismo que el de arriba.

	[Header("PostProcess Settings")]
	public PostProcessProfile myProfile;
	public Color NormalVignette;
	public Color DamageVignette;
	public float _vignetteEffectOriginal;

	[HideInInspector]
	public InputController controller;

	[Header("Player Stats")]
	public int Life;
	public int MaxLife;

	public float BaseMovementSpeed;
	public float RunningSpeed;
	public float HorizontalSpeed;
	public float VerticalSpeed;
	public bool isRunning = false;

	[Header("Default Equipment")]
	public Gun MainGun;

	float _currentMovementSpeed;
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

		var vignetteSet = myProfile.GetSetting<Vignette>();
		vignetteSet.color.value = NormalVignette;
		vignetteSet.intensity.Override(_vignetteEffectOriginal);
		vignetteSet.active = true;
	}

	private void Start()
	{
		//Me registro al manager.
		GameModelManager.instance.PlayerList.Add(gameObject);

		//Suscribo mis eventos.
		controller.Bind(Input.GetButton, Input.GetAxis, "Horizontal", "Vertical", MoveInAxeses);
		controller.Bind(Input.GetButtonUp, Input.GetAxis,"Horizontal","Vertical", Quiet);

		controller.Bind(Input.GetButtonDown, "Run", running);
		controller.Bind(Input.GetButtonUp, "Run", running);
		controller.Bind(Input.GetButtonUp, "Jump", Jump);

		controller.Bind(Input.GetAxis, "Mouse X", "Mouse Y", RotateCamera);

		controller.Bind(Input.GetButton, "Shoot", Shoot);
		controller.Bind(Input.GetButtonDown, "Reload", Reload);
		controller.Bind(Input.GetButtonUp, "Pause", GameModelManager.instance.Pause);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void MoveInAxeses(float horizontal, float vertical)
	{
		if (!PlayerAnims.GetBool("IsWalking"))
			PlayerAnims.SetBool("IsWalking", true);
		//Movement Speed.
		_currentMovementSpeed = isRunning ? RunningSpeed : BaseMovementSpeed;

		//Fix movimiento en Diagonal.
		var movement = (transform.right * horizontal) + (transform.forward * vertical);
		transform.position +=  movement * _currentMovementSpeed * Time.deltaTime;
	}
	public void Quiet(float Horizontal, float Vertical)
	{
		//print("Horizontal is :" + Horizontal + " and Vertical is: " + Vertical + ".");
		if ((int)Horizontal == 0 && (int)Vertical == 0) PlayerAnims.SetBool("IsWalking", false);
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
	}
	public void Jump()
	{
		if (Jumps == 1)
		{
			rb.AddForce(transform.up * 400);
			Jumps--;
		}
	}
	public void Shoot()
	{
		_currentWeapon.Shoot();
	}
	public void Reload()
	{
		_currentWeapon.Reload();
	}

	public void SwitchToMainWeapon()
	{
		_currentWeapon = MainGun;
		_currentWeapon.OnSelect();
	}

	public void AddDamage(int Damage)
	{
		CameraAnims.SetTrigger("CameraShake");
		Life -= Damage;
		StartCoroutine(ShowDamage());

		if (Life <= 0)
		{
			print("Estas muerto");
			OnDie();
			game.EndGame();
		}
		game.UpdateLife((float)Life/MaxLife);
		print("Recibiste daño!");
	}
	public void OnDie()
	{
		ResetPostProcessing(myProfile.GetSetting<Vignette>(), myProfile.GetSetting<ChromaticAberration>(), myProfile.GetSetting<Grain>());
	}

	IEnumerator ShowDamage()
	{
		var MyVignette = myProfile.GetSetting<Vignette>();
		var MyChromaticAberration = myProfile.GetSetting<ChromaticAberration>();
		var MyGrain = myProfile.GetSetting<Grain>();

		MyVignette.enabled.overrideState = true;
		MyChromaticAberration.active = true;
		MyGrain.active = true;

		//Intensidad va de 0 a 1;
		MyVignette.intensity.Override(0.48f);
		MyVignette.color.value = DamageVignette;

		while (MyVignette.intensity > _vignetteEffectOriginal)
		{
			float intencity = MyVignette.intensity;
			intencity -= 0.008f;
			MyVignette.intensity.Override(intencity);
			yield return new WaitForEndOfFrame();
		}

		ResetPostProcessing(MyVignette,MyChromaticAberration,MyGrain);
	}
	public void ResetPostProcessing(Vignette vignette, ChromaticAberration chromaticAberration, Grain grain)
	{
		chromaticAberration.active = false;
		grain.active = false;

		vignette.intensity.Override(_vignetteEffectOriginal);
		vignette.color.value = NormalVignette;
	}

#if UNITY_EDITOR
	void OnApplicationQuit()
	{
		ResetPostProcessing(myProfile.GetSetting<Vignette>(), myProfile.GetSetting<ChromaticAberration>(), myProfile.GetSetting<Grain>());
	}
#endif

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 8) Jumps = 1;
	}
}
