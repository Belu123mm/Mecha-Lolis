using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
	public GameModelManager game;
	public Camera firstPersonCamera;

	public float BaseMovementSpeed;
	public float RunningSpeed;
	public float HorizontalSpeed;
	public float VerticalSpeed;
	public bool isRunning = false;

	float _currentMovementSpeed;
	Rigidbody rb;

	private void Awake()
	{
		game = new GameModelManager();
		rb = GetComponent<Rigidbody>();
	}
	private void Start()
	{
		//Suscribo mis eventos.
		game.AddHorizontalEvent( InputEventType.Continious, MoveHorizontal );
		game.AddVerticalEvent( InputEventType.Continious, MoveVertical);
		game.AddSimpleInputEvent( InputEventType.OnBegin, KeyCode.LeftShift, running);
		game.AddSimpleInputEvent( InputEventType.OnRelease, KeyCode.LeftShift, running);
		game.AddMouseTrack(RotateCamera);
	}

	public void MoveHorizontal(float dir)
	{
		_currentMovementSpeed = isRunning ? RunningSpeed : BaseMovementSpeed;
		transform.position += transform.right * dir * _currentMovementSpeed * Time.deltaTime;
	}
	public void MoveVertical(float dir)
	{
		_currentMovementSpeed = isRunning ? RunningSpeed : BaseMovementSpeed;
		transform.position += transform.forward * dir * _currentMovementSpeed * Time.deltaTime;
	}
	public void running()
	{
		isRunning = !isRunning;
	}

	public void RotateCamera(float x, float y)
	{
		var h = HorizontalSpeed * x;
		var v = VerticalSpeed * y;

		transform.Rotate(0, h, 0);
		firstPersonCamera.transform.Rotate(-v, 0, 0);
	}

	public void Jump()
	{
		//Añadir lógica del salto.
		//Tenemos el rb.
	}

}
