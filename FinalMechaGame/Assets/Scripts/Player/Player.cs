using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamageable {
	public GameModelManager game;
    public GameObject EnemyBulletPrefab;
	public Transform firstPersonContainer;
    public Animator CameraAnims;
	public Animator anim;
    public PostProcessingProfile myProfile;
    public Color NormalVignette;
    public Color DamageVignette;
    public float VignetteTime;
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
	//Rigidbody rb;

	private void Awake()
	{
		game = new GameModelManager();
        game.BulletParent = GameObject.Find("Bullets");
		game.InitializeEnemyBulletPool(60, EnemyBulletPrefab, EnemyBullet.InitializeBullet, EnemyBullet.DeactivateBullet, true);
        _vignetteEffectMax = myProfile.vignette.settings.intensity;
		//rb = GetComponent<Rigidbody>();
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
		var h = HorizontalSpeed * x;
		var v = VerticalSpeed * y;

		transform.Rotate(0, h, 0);
		firstPersonContainer.Rotate(-v, 0, 0);
	}

	public void running() { isRunning = !isRunning; }

	public void Jump()
	{
		//Añadir lógica del salto.
		//Tenemos el rb.
	}

	public void AddDamage(int Damage)
	{
		CameraAnims.SetTrigger("CameraShake");
		Life -= Damage;
		StartCoroutine(ShowDamage());

		if (Life <= 0)
		{
			print("Estas muerto");
			game.EndGame();
		}
		game.UpdateLife((float)Life/MaxLife);
		print("Recibiste daño!");
	}

    IEnumerator ShowDamage()
    {
        myProfile.vignette.enabled = true;
        myProfile.chromaticAberration.enabled = true;
        myProfile.grain.enabled = true;
        var sets = myProfile.vignette.settings;
        sets.color = DamageVignette;
        while (sets.intensity >= 0)
        {
            sets.intensity -= 0.008f;
            myProfile.vignette.settings = sets;
            yield return new WaitForEndOfFrame();
        }

        myProfile.vignette.enabled = false;
        myProfile.chromaticAberration.enabled = false;
        myProfile.grain.enabled = false;
        sets.intensity = _vignetteEffectMax;
        sets.color = NormalVignette;
        myProfile.vignette.settings = sets;
    }
}
