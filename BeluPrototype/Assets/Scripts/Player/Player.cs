using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
	public GameModelManager game;

	public float BaseMovementSpeed;
	public float RunningSpeed;
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
        game.AddHorizontalMovement(MoveHorizontal);
        game.AddVerticalMovement(MoveVertical);
        game.AddBeginInputEvent(KeyCode.LeftShift, running);
        game.AddReleaseInputEvent(KeyCode.LeftShift, running);
    }

    public void MoveHorizontal(float dir)
	{
		_currentMovementSpeed = isRunning ? RunningSpeed : BaseMovementSpeed;
		transform.position += transform.forward * dir * _currentMovementSpeed * Time.deltaTime;
	}
	public void MoveVertical(float dir)
	{
		_currentMovementSpeed = isRunning ? RunningSpeed : BaseMovementSpeed;
		transform.position += transform.right * dir * _currentMovementSpeed * Time.deltaTime;
	}
	public void running()
	{
		isRunning = !isRunning;
	}
	public void Jump()
	{
		//Añadir lógica del salto.
		//Tenemos el rb.
	}

}
