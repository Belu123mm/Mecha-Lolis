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

    GameObject activeGrenade;
    float TimeToRecast;
    int uses = 0;
    bool Triggereable = false;
    bool grenadeIsSelected = false;

    private void Start()
    {
        TimeToRecast = Cooldown;
    }
    // Update is called once per frame
    void Update () {

        //Cooldown & Tp
        if (Triggereable == true)
        {
            CooldownCount();
            if (Input.GetKeyDown(KeyCode.E) && uses == 1)
            {
                uses = 0;
                transform.position = activeGrenade.transform.localPosition;
            }
        }

        //Selección.
        if (Input.GetKeyDown(KeyCode.Alpha3) && !Triggereable && uses == 0)
        {
            TimeToRecast = Cooldown;
            Triggereable = false;
            grenadeIsSelected = !grenadeIsSelected;
            print(grenadeIsSelected ?"Granada esta seleccionada" : "La Granda fue deseleccionada");
        }

        //Disparo
        if (grenadeIsSelected && Input.GetMouseButtonUp(0) && uses == 0)
        {
            Triggereable = true;
            grenadeIsSelected = false;
            uses = 1;
            activeGrenade = Instantiate(grenadePrefab,transform.position + new Vector3(1,1,0),Quaternion.identity);
            activeGrenade.GetComponent<Rigidbody>().AddForce(Pointer.transform.forward * force, ForceMode.Impulse);
        }
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
}
