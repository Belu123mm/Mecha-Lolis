using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamageable {
	public GameModelManager game; //Esto deberia sacarlo de acá.
	public GameObject EnemyBulletPrefab; //Esto igual.
	public Transform firstPersonContainer;
	public Animator CameraAnims; //Quizas esto ponerlo en otro componente.
	public Animator anim; //Mismo que el de arriba.
	//public PostProcessingProfile myProfile; //Esto quizás vuele.
	public Color NormalVignette; //Mismo de arriba.
	public Color DamageVignette; //Mismo de arriba.
    public float VignetteTime; //Mismo de arriba.

    [Header("Player Stats")]
    public int Life;
	public int MaxLife;

	public float BaseMovementSpeed;
	public float RunningSpeed;
	public float HorizontalSpeed;
	public float VerticalSpeed;
	public bool isRunning = false;

	int[] axeses = { 0, 0 };
	float _currentMovementSpeed;
	float _vignetteEffectMax;
	float rotation = 0;
	int Jumps = 1;
	Rigidbody rb;
	private void Awake()
	{
		game = new GameModelManager();
		game.BulletParent = GameObject.Find("Bullets");
		game.InitializeEnemyBulletPool(60, EnemyBulletPrefab, EnemyBullet.InitializeBullet, EnemyBullet.DeactivateBullet, true);
		//_vignetteEffectMax = myProfile.vignette.settings.intensity;
		rb = GetComponent<Rigidbody>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	private void Start()
	{
		//Suscribo mis eventos.
		game.AddAxisEvent(InputEventType.OnBegin, Axeses.Horizontal, Moving);
		game.AddAxisEvent(InputEventType.Continious, Axeses.Horizontal, MoveInAxeses);
		game.AddAxisEvent(InputEventType.OnRelease, Axeses.Horizontal, Quiet);

		game.AddAxisEvent(InputEventType.OnBegin, Axeses.Vertical, Moving);
		game.AddAxisEvent(InputEventType.Continious, Axeses.Vertical, MoveInAxeses);
		game.AddAxisEvent(InputEventType.OnRelease, Axeses.Vertical, Quiet);

		game.AddSimpleInputEvent(InputEventType.OnBegin, KeyCode.LeftShift, running);
		game.AddSimpleInputEvent(InputEventType.OnRelease, KeyCode.LeftShift, running);
		game.AddSimpleInputEvent(InputEventType.OnRelease, KeyCode.Space, Jump);

		game.AddMouseTrack(RotateCamera);

		game.StartGame();
	}

	public void MoveInAxeses(float dir, int Axis)
	{
		_currentMovementSpeed = isRunning ? RunningSpeed : BaseMovementSpeed;

		if (Axis == 0)
			transform.position += transform.right * dir * _currentMovementSpeed * Time.deltaTime;
		if (Axis == 1)
			transform.position += transform.forward * dir * _currentMovementSpeed * Time.deltaTime;
	}

	public void Moving(float dir, int Axis)
	{
		if (Axis == 1) axeses[0] = 1;
		if (Axis == 2) axeses[1] = 1;
		//print("Estoy moviendome");
			anim.SetBool("IsWalking", true);
	}

	public void Quiet(float dir, int Axis)
	{
		if (Axis == 1) axeses[0] = 0;
		if (Axis == 2) axeses[1] = 0;
		if (axeses[0] == 0 && axeses[1] == 0)
		{
			//print("Estoy Quieto");
			anim.SetBool("IsWalking", false);
		}
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

	public void running() { isRunning = !isRunning; }

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
