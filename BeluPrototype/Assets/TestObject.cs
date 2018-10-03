using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour {
    [Range(0,1)]public float value;
    public GameModelManager Game;

    private void Awake()
    {
        Game = new GameModelManager();
        //Añado un evento de teclado;
        Game.controller.AddInputEvent(KeyCode.W, DecirBla);
    }

    // Update is called once per frame
    void Update ()
    {
        //Lo ideal seria que esta linea vaya al ejecutarse un evento.
        Game.gameView.LifeDisplay = value;
	}

    public void DecirBla()
    {
        print("YoloNigga");
    }
}
