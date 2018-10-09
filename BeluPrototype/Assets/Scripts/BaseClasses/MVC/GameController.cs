using System;
using System.Collections.Generic;
using UnityEngine;

//Necesitamos poder exponer y mostar esto por inspector.
//De manera que podamos crear a mano cada evento, darle un contexto y luego mandarlo.
[RequireComponent(typeof(GameManagerView))]
public class GameController : MonoBehaviour
{
	public List<KeyCode> keys; //Keys que vamos a trackear.
	public Dictionary<KeyCode, Action> OnKeyCode;//Eventos simples continuos.
	public Dictionary<KeyCode, Action> OnBeginKeyCode;//Eventos simples.
	public Dictionary<KeyCode, Action> OnRealeaseKeyCode; //Final de eventos simples.

	public Dictionary<int, Action> OnBeginMouse;
	public Dictionary<int, Action> OnMouse;
	public Dictionary<int, Action> OnReleaseMouse;

	//El int: 1 = Horizontal, 2 = vertical.
	public Dictionary<int,Action<float>> OnGetAxes;
	public Dictionary<int,Action<float>> OnBeginAxes;
	public Dictionary<int,Action<float>> OnReleaseAxes;

	List<Action> SimpleExecutionList;
	List<Tuple<float,Action<float>>> AxesExecutionList;
    public void Awake()
	{
		//Inicializo los Diccionarios.
		keys = new List<KeyCode>();
		OnKeyCode = new Dictionary<KeyCode, Action>();
		OnBeginKeyCode = new Dictionary<KeyCode, Action>();
		OnRealeaseKeyCode = new Dictionary<KeyCode, Action>();

		OnMouse = new Dictionary<int, Action>() { { 0, delegate { print("Clic 1"); } }, { 1, delegate { print("Clic 2"); } } };
		OnBeginMouse = new Dictionary<int, Action>() { { 0, delegate { print("Clic start 1"); } }, { 1, delegate { print("Clic start 2"); } } };
		OnReleaseMouse = new Dictionary<int, Action>() { { 0, delegate { print("Clic release 1"); } }, { 1, delegate { print("Clic release 2"); } } };

		OnGetAxes = new Dictionary<int, Action<float>>();
		OnBeginAxes = new Dictionary<int, Action<float>>();
		OnReleaseAxes = new Dictionary<int, Action<float>>();

		SimpleExecutionList = new List<Action>();
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
		foreach (var key in keys)
		{
			if (Input.GetKeyDown(key))
				SimpleExecutionList.Add(OnBeginKeyCode[key]);
			else if (Input.GetKey(key))
				SimpleExecutionList.Add(OnBeginKeyCode[key]);

			if (Input.GetKeyDown(key))
				SimpleExecutionList.Add(OnRealeaseKeyCode[key]);
		}

		if (Input.GetMouseButtonDown(0)) SimpleExecutionList.Add(OnBeginMouse[0]);
		else if (Input.GetMouseButton(0)) SimpleExecutionList.Add(OnMouse[0]);
		if (Input.GetMouseButtonUp(0)) SimpleExecutionList.Add(OnReleaseMouse[0]);

		if (Input.GetMouseButtonDown(1)) SimpleExecutionList.Add(OnBeginMouse[1]);
		else if (Input.GetMouseButton(1)) SimpleExecutionList.Add(OnMouse[1]);
		if (Input.GetMouseButtonUp(1)) SimpleExecutionList.Add(OnReleaseMouse[1]);


		if (Input.GetButtonDown("Horizontal"))
			if (OnBeginAxes.ContainsKey(1))
                AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Horizontal"), OnBeginAxes[1]));
            else
                print("Horizontal no existe");
        //if (Input.GetButton("Horizontal"))
        //	if (OnBeginAxes.ContainsKey(1))
        //		AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Horizontal"), OnGetAxes[1]));
        //if (Input.GetButtonUp("Horizontal"))
        //	if (OnBeginAxes.ContainsKey(1))
        //		AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Horizontal"), OnReleaseAxes[1]));


        //if (Input.GetButtonDown("Vertical"))
        //	if (OnBeginAxes.ContainsKey(2))
        //		AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Vertical"), OnBeginAxes[2]));
        //if (Input.GetButton("Vertical"))
        //	if (OnBeginAxes.ContainsKey(2))
        //		AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Vertical"), OnGetAxes[2]));
        //if (Input.GetButtonDown("Vertical"))
        //	if (OnBeginAxes.ContainsKey(2))
        //		AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Vertical"), OnReleaseAxes[2]));

        if (SimpleExecutionList.Count > 0) foreach (var item in SimpleExecutionList) item();
		SimpleExecutionList.Clear();
	}	
}
