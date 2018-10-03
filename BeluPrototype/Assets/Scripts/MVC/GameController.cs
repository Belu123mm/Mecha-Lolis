using System;
using System.Collections.Generic;
using UnityEngine;

//Necesitamos poder exponer y mostar esto por inspector.
//De manera que podamos crear a mano cada evento, darle un contexto y luego mandarlo.
[RequireComponent(typeof(GameManagerView))]
public class GameController : MonoBehaviour
{
    public List<KeyCode> keys; //Keys que vamos a trackear.
    public Dictionary<KeyCode, Action> KeyCodeEvents;//Eventos que vamos a llamar.
    public Dictionary<int, Action> MouseEvents;
    public bool continius = false;

    List<Action> ExecutionList = new List<Action>();//La inicializamos una sola vez.
    public GameController()
    {
        keys = new List<KeyCode>();
        KeyCodeEvents = new Dictionary<KeyCode, Action>();
        MouseEvents = new Dictionary<int, Action>();
    }

    //Chequeo el input del jugador.
    private void Update()
    {
        //Problema, se ejecutara una sola vez.

        //Posible solucion, guardar el evento con una key que sea un int.
        //Guardamos una tupla con un keycode y un Action.
        //Creamos una lista de Actions.
        //Por cada valor del diccionario, chequemos que el keycode coincida.
        //Creamos una función GetKey, GetKeyUp, GetKeyDown, GetKeyUp.
        //Si coindiden dentro de la función añadimos el evento(Action) en la lista de ejecución. En caso de que no exista ya.
        //Al final ejecutamos Solo las funciones que estan listadas.
        foreach (var item in keys)
        {
            if (Input.GetKeyDown(item))
                ExecutionList.Add(KeyCodeEvents[item]);

            if (Input.GetKey(item) && continius && !ExecutionList.Contains(KeyCodeEvents[item]))
                ExecutionList.Add(KeyCodeEvents[item]);
        }

        if (Input.GetMouseButton(0)) ExecutionList.Add(MouseEvents[0]);
        if (Input.GetMouseButton(1)) ExecutionList.Add(MouseEvents[1]);

        if (ExecutionList.Count > 0) foreach (var item in ExecutionList) item();
        ExecutionList.Clear();
    }

    public void AddInputEvent(KeyCode Input, Action Evento)
    {
        keys.Add(Input);
        KeyCodeEvents.Add(Input, Evento);
    }

    public void AddMouseEvent(int mouseButton, Action Evento)
    {
        //Añado el evento a un Dictionary.
        MouseEvents.Add(mouseButton, Evento);
    }
}

