﻿using UnityEngine;

//Este item tiene 3 partes, la primera es la selección, la segunda es el enfriamiento, y la ultima es el teletransportado con reseteo.
//Lo ideal es que la granada misma tenga el conteo del enfriamiento para ahorrar código.
//Asi por eventos, avisamos al player cuando el enfriamiento es posible nuevamente, por ultimo la propia granada podria ser recogible.
//El enfriamiento marcaria el tanto el tiempo de activación del teletransportado. Por otro lado pickear el la granada reduciría el enfriamiento.

public class TPGranade : MonoBehaviour {
    public GameModelManager game;

    public float Cooldown;
    public GameObject Pointer;
    public GameObject grenadePrefab;
    public float force;

    GameObject activeGrenade;
    float TimeToRecast;
    int uses = 0;
    bool Triggereable = false;

    private void Start()
    {
        TimeToRecast = Cooldown;
        game = new GameModelManager();

        GameModelManager.instance.controller.AddInputEvent(KeyCode.E, Teleport);
        GameModelManager.instance.controller.AddInputEvent(KeyCode.Alpha3, Selection);
        GameModelManager.instance.controller.AddMouseEvent(0, Shoot);
    }

    private void Update()
    {
        if (!Triggereable) CooldownCount();
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

    void Teleport()
    {
        //(KeyCode.E)
        if (uses == 1)
        {
            uses = 0;
            transform.position = activeGrenade.transform.localPosition;
        }
    }

    void Selection()
    {
        //Selección.
        //(KeyCode.Alpha3)
        if (Triggereable && uses == 0)
        {
            TimeToRecast = Cooldown;
            Triggereable = true;
            print(Triggereable ? "Granada esta seleccionada" : "La Granda fue deseleccionada");
        }
    }

    void Shoot()
    {
        //Input.GetMouseButtonUp(0)
        if (!Triggereable && uses == 0)
        {
            uses = 1;
            activeGrenade = Instantiate(grenadePrefab, transform.position + new Vector3(1, 1, 0), Quaternion.identity);
            activeGrenade.GetComponent<Rigidbody>().AddForce(Pointer.transform.forward * force, ForceMode.Impulse);
        }
    }
}
