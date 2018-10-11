using UnityEngine;

//Este item tiene 3 partes, la primera es la selección, la segunda es el enfriamiento, y la ultima es el teletransportado con reseteo.
//Lo ideal es que la granada misma tenga el conteo del enfriamiento para ahorrar código.
//Asi por eventos, avisamos al player cuando el enfriamiento es posible nuevamente, por ultimo la propia granada podria ser recogible.
//El enfriamiento marcaria el tanto el tiempo de activación del teletransportado. Por otro lado pickear el la granada reduciría el enfriamiento.

public class TPGranade : MonoBehaviour {
	public float Cooldown;
	public GameObject Pointer;
	public GameObject grenadePrefab;
	public float force;
	public bool Triggereable = false;

	GameObject activeGrenade;
	float TimeToRecast;
	int uses = 0;

	private void Start()
	{
		TimeToRecast = Cooldown;
		
		//Action de la clase.
		GameModelManager.instance.AddSimpleInputEvent(InputEventType.OnBegin, KeyCode.E, Teleport);
		GameModelManager.instance.AddSimpleInputEvent(InputEventType.OnBegin, KeyCode.Alpha3, Selection);
		GameModelManager.instance.AddMouseEvent(InputEventType.OnBegin, 0, ThrowGrenade);
	}

	private void Update()
	{
		if (Triggereable && uses == 0) CooldownCount();
	}

	void CooldownCount()
	{
		TimeToRecast -= Time.deltaTime;
		print(TimeToRecast);
		if (TimeToRecast <= 0)
		{
			print("Habilidad lista otra vez");
			TimeToRecast = Cooldown;
			Triggereable = false;
			uses = 0;
		}
	}


	void Selection()
	{
		if (!Triggereable)
		{
            uses = 1;
			TimeToRecast = Cooldown;
			Triggereable = true;
			print(Triggereable ? "Granada esta seleccionada" : "La Granda fue deseleccionada");
		}
	}

	void ThrowGrenade()
	{
		if (Triggereable && uses == 1)
		{
			activeGrenade = Instantiate(grenadePrefab, transform.position + new Vector3(1, 1, 0), Quaternion.identity);
			activeGrenade.GetComponent<Rigidbody>().AddForce(Pointer.transform.forward * force, ForceMode.Impulse);
		}
	}

	void Teleport()
	{
        print("Teleport y uses: " + uses);
		if (uses == 1)
		{
			uses = 0;
			transform.position = activeGrenade.transform.localPosition;
		}
	}
}
