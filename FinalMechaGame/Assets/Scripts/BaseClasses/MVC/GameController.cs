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
	public Dictionary<KeyCode, Action> OnReleaseKeyCode; //Final de eventos simples.

	public Dictionary<int, Action> OnBeginMouse;
	public Dictionary<int, Action> OnMouse;
	public Dictionary<int, Action> OnReleaseMouse;

	public Action<float, float> MouseAxisTrack = delegate { };

	//El int: 1 = Horizontal, 2 = vertical.
	int _vertical = (int)Axeses.Vertical;
	int _horizontal = (int)Axeses.Horizontal;
	public Dictionary<int,Action<float,int>> OnGetAxes;
	public Dictionary<int,Action<float,int>> OnBeginAxes;
	public Dictionary<int,Action<float,int>> OnReleaseAxes;

	List<Action> SimpleExecutionList;
	List<Tuple<float, int ,Action<float,int>>> AxesExecutionList;

	public void Awake()
	{
		//Inicializo los Diccionarios.
		keys = new List<KeyCode>();
		OnKeyCode = new Dictionary<KeyCode, Action>();
		OnBeginKeyCode = new Dictionary<KeyCode, Action>();
		OnReleaseKeyCode = new Dictionary<KeyCode, Action>();

		OnMouse = new Dictionary<int, Action>() { { 0, delegate { } }, { 1, delegate { } } };
		OnBeginMouse = new Dictionary<int, Action>() { { 0, delegate { } }, { 1, delegate { } } };
		OnReleaseMouse = new Dictionary<int, Action>() { { 0, delegate { } }, { 1, delegate { } } };

		OnGetAxes = new Dictionary<int, Action<float,int>>() { { 1, delegate { } } };
		OnBeginAxes = new Dictionary<int, Action<float,int>>() { { 1, delegate { } } };
		OnReleaseAxes = new Dictionary<int, Action<float,int>>() { { 1, delegate { } } };

		SimpleExecutionList = new List<Action>();
		AxesExecutionList = new List<Tuple<float, int , Action<float,int>>>();
	}

	//Chequeo el input del jugador.
	private void Update()
	{
		foreach (var key in keys)
		{
			if (Input.GetKeyUp(key) && OnReleaseKeyCode.ContainsKey(key))
				SimpleExecutionList.Add(OnReleaseKeyCode[key]);
			if (Input.GetKeyDown(key) && OnBeginKeyCode.ContainsKey(key))
				SimpleExecutionList.Add(OnBeginKeyCode[key]);
			if (Input.GetKey(key) && OnKeyCode.ContainsKey(key))
				SimpleExecutionList.Add(OnKeyCode[key]);
		}

		if (Input.GetMouseButtonDown(0)) SimpleExecutionList.Add(OnBeginMouse[0]);
		else if (Input.GetMouseButton(0)) SimpleExecutionList.Add(OnMouse[0]);
		if (Input.GetMouseButtonUp(0)) SimpleExecutionList.Add(OnReleaseMouse[0]);

		if (Input.GetMouseButtonDown(1)) SimpleExecutionList.Add(OnBeginMouse[1]);
		else if (Input.GetMouseButton(1)) SimpleExecutionList.Add(OnMouse[1]);
		if (Input.GetMouseButtonUp(1)) SimpleExecutionList.Add(OnReleaseMouse[1]);


		if (Input.GetButtonDown("Horizontal"))
			AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Horizontal"), _horizontal, OnBeginAxes[_horizontal]));
		else if (Input.GetButton("Horizontal") && OnGetAxes.ContainsKey(_horizontal))
			AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Horizontal"), _horizontal, OnGetAxes[_horizontal]));
		if (Input.GetButtonUp("Horizontal") && OnBeginAxes.ContainsKey(_horizontal))
			AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Horizontal"), _horizontal, OnReleaseAxes[_horizontal]));

		if (Input.GetButtonDown("Vertical") && OnBeginAxes.ContainsKey(_vertical))
			AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Vertical"), _vertical, OnBeginAxes[_vertical]));
		else if (Input.GetButton("Vertical") && OnGetAxes.ContainsKey(_vertical))
			AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Vertical"), _vertical, OnGetAxes[_vertical]));
		if (Input.GetButtonUp("Vertical") && OnReleaseAxes.ContainsKey(_vertical))
			AxesExecutionList.Add(Tuple.Create(Input.GetAxis("Vertical"), _vertical, OnReleaseAxes[_vertical]));

		//-------------------------------------Ejecucion-----------------------------------------------------------
		if (SimpleExecutionList.Count > 0) foreach (var item in SimpleExecutionList) item();
		SimpleExecutionList.Clear();

		if (AxesExecutionList.Count > 0) foreach (var element in AxesExecutionList) element.Item3(element.Item1, element.Item2);
		AxesExecutionList.Clear();

		MouseAxisTrack(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
	}
}
