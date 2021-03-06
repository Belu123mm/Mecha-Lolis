﻿using System;
using UnityEngine;
/// <summary>
/// Esta clase maneja datos nativos. Clases y Operaciones del juego.
/// </summary>
public class GameModelManager
{
	public static GameModelManager instance;
	public Pool<GameObject> PlayerBulletPool;
	public Pool<GameObject> EnemyBulletPool;
	public GameObject BulletParent;
	public float Points = 0;

	GameController _controller;
	GameManagerView _gameView;
	SceneManagement _scenes;

	//Tiempo de juego.
	//Recuento de oleadas.
	//Lista de Spawners.(?).
	//Lista de enemigos. (?).
	//public static List<Player> Player; //PlayerSerializable.

	public GameModelManager()
	{
		if (instance == null) instance = this;
		_gameView = UnityEngine.Object.FindObjectOfType<GameManagerView>();
		_controller = UnityEngine.Object.FindObjectOfType<GameController>();
		_scenes = UnityEngine.Object.FindObjectOfType<SceneManagement>();
		UpdatePoints();
        BulletParent = UnityEngine.GameObject.Find("Bullets");
	}

	public void InitializeEnemyBulletPool(int ammount,GameObject bulletPrefab, Action<GameObject> init, Action<GameObject> finit, bool isDinamic = false)
	{
		//Factory
		Func<GameObject> factory = () => 
		{ return UnityEngine.Object.Instantiate(
			bulletPrefab, 
			Vector3.zero, 
			Quaternion.identity,
			BulletParent.transform);
		};

		//Bullet Pool
		EnemyBulletPool = new Pool<GameObject>(ammount, factory, init, finit
			, isDinamic);

		//Método de reemplazo para Destroy()
		EnemyBullet.OnDeactivate = EnemyBulletPool.ReturnObjectToPool;
		//MonoBehaviour.print("Ammount of bullets is: " + EnemyBulletPool.Count);
	}

	/// <summary>
	/// Inicia el juego.
	/// </summary>
	public void StartGame()
	{
		//Acá iría todo lo que se hace cuando el juego Empieza.
		//Si el juego comienza desde cero no cargamos nada, pero si el jugador lo desea puede cargar una partida en cualquier momento.
	}

	/// <summary>
	/// Termina el juego.
	/// </summary>
	public void EndGame()
	{
        //Acá iría todo lo que se hace cuando el juego termina.
        //aca tendriamos una instancia de "SceneManager" por ejemplo y ejecutar su función.
        //Por ejemplo, decirle a nuestro view que habra la pantalla de derrota. Dejandonos cargar la ultima partida.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
		_scenes.LoadDefeatScene();
	}

	/// <summary>
	/// Carga una partida anterior.
	/// </summary>
	public void LoadGame()
	{
		//Des-Serializa el estado del juego y lo carga en memoria.
	}

	/// <summary>
	/// Guarda los datos de la partida actual.
	/// </summary>
	public void SaveGame()
	{
		//Serializa el estado del juego y lo guarda en disco.
	}

	public void UpdateLife(float life)
	{
		if (life < 0)
			_gameView.LifeDisplay = 0;
		else
			_gameView.LifeDisplay = life;
	}
	public void UpdatePoints()
	{
		_gameView.PointDisplay = Points.ToString();
	}
	public void UpdateBullets(float bullets, float maxBullets)
	{
		_gameView.BulletDisplay = bullets.ToString();

		if (maxBullets.ToString() != _gameView.MaxBulletDisplay)
			_gameView.MaxBulletDisplay = maxBullets.ToString();
	}

	#region Eventos Comunes
	//Añadir eventos de teclado
	/// <summary>
	/// Añade un evento de teclado que responde al Axis "Horizontal".
	/// </summary>
	/// <param name="type">1 = OnBegin, 2 = Continuous, 3 = OnRelease.</param>
	/// <param name="evento">Evento a ejecutar.</param>
	public void AddAxisEvent(InputEventType type, Axeses Axis, Action<float,int> evento)
	{
		switch (type)
		{
			case InputEventType.OnBegin:

				if (_controller.OnBeginAxes.ContainsKey((int)Axis)) _controller.OnBeginAxes[(int)Axis] += evento;
				else _controller.OnBeginAxes.Add((int)Axis, evento);
				break;
			case InputEventType.Continious:
				if (!_controller.OnGetAxes.ContainsKey((int)Axis)) _controller.OnGetAxes.Add((int)Axis, evento);
				else _controller.OnGetAxes[(int)Axis] += evento;
				break;
			case InputEventType.OnRelease:
				if (!_controller.OnReleaseAxes.ContainsKey((int)Axis)) _controller.OnReleaseAxes.Add((int)Axis, evento);
				else _controller.OnReleaseAxes[(int)Axis] += evento;
				break;
			default:
				break;
		}
	}
	/// <summary>
	/// Añade un evento de teclado que responde a un KeyCode.
	/// </summary>
	/// <param name="type">1 = OnBegin, 2 = Continuous, 3 = OnRelease.</param>
	/// <param name="Input">Tecla que identifica al evento.</param>
	/// <param name="Evento">Evento a ejecutar.</param>
	public void AddSimpleInputEvent(InputEventType type, UnityEngine.KeyCode Input, Action Evento)
	{
		if (!_controller.keys.Contains(Input)) _controller.keys.Add(Input);

		switch (type)
		{
			case InputEventType.OnBegin:
				if (!_controller.OnBeginKeyCode.ContainsKey(Input)) _controller.OnBeginKeyCode.Add(Input, Evento);
				else _controller.OnBeginKeyCode[Input] += Evento;
				break;
			case InputEventType.Continious:
				if (!_controller.OnKeyCode.ContainsKey(Input)) _controller.OnKeyCode.Add(Input, Evento);
				else _controller.OnKeyCode[Input] += Evento;
				break;
			case InputEventType.OnRelease:
				if (!_controller.OnReleaseKeyCode.ContainsKey(Input)) _controller.OnReleaseKeyCode.Add(Input, Evento);
				else _controller.OnReleaseKeyCode[Input] += Evento;
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Añade un evento que responde al mouse.
	/// </summary>
	/// <param name="type">1 = OnBegin, 2 = Continuous, 3 = OnRelease.</param>
	/// <param name="mouseButton">0 = izquierdo, 1 = derecho.</param>
	/// <param name="Evento">Evento a ejecutar.</param>
	public void AddMouseEvent(InputEventType type, int mouseButton, Action Evento)
	{
		switch (type)
		{
			case InputEventType.OnBegin:
				if (!_controller.OnBeginMouse.ContainsKey(mouseButton)) _controller.OnBeginMouse.Add(mouseButton, Evento);
				else _controller.OnBeginMouse[mouseButton] += Evento;
				break;
			case InputEventType.Continious:
				if (!_controller.OnMouse.ContainsKey(mouseButton))_controller.OnMouse.Add(mouseButton, Evento);
				else _controller.OnMouse[mouseButton] += Evento;
				break;
			case InputEventType.OnRelease:
				if (!_controller.OnReleaseMouse.ContainsKey(mouseButton)) _controller.OnReleaseMouse.Add(mouseButton, Evento);
				else _controller.OnReleaseMouse[mouseButton] += Evento;
				break;
			default:
				break;
		}
	}
	/// <summary>
	/// Añade un evento que requiera los ejes del mouse en x e y.
	/// </summary>
	/// <param name="MouseTrack">Evento.</param>
	public void AddMouseTrack(Action<float,float> MouseTrack)
	{
		_controller.MouseAxisTrack += MouseTrack;
	}
	#endregion
}

/// <summary>
/// Determina el tiempo de ejecución de un evento relacionado al Input.
/// </summary>
public enum InputEventType
{
	OnBegin,
	Continious,
	OnRelease
}
public enum Axeses
{
	Horizontal,
	Vertical
}
