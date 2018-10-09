//using System.Collections;
//using System.Collections.Generic;

using System;
/// <summary>
/// Esta clase maneja datos nativos. Clases y Operaciones del juego.
/// </summary>
public class GameModelManager
{
	public static GameModelManager instance;
	public GameController controller;
	public GameManagerView gameView;

	public float Points = 0;
	//Tiempo de juego.
	//Recuento de oleadas.
	//Lista de Spawners.(?).
	//Lista de enemigos. (?).
	//public static List<Player> Player; //PlayerSerializable.

	public GameModelManager()
	{
		instance = this;
		gameView = UnityEngine.Object.FindObjectOfType<GameManagerView>();
		controller = UnityEngine.Object.FindObjectOfType<GameController>();
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

	#region Eventos Comunes
	//Añadir eventos de teclado
	public void AddHorizontalMovement(Action<float> evento)
	{
		controller.OnGetAxes.Add(1, evento);
	}
	public void AddVerticalMovement(Action<float> evento)
	{
		controller.OnGetAxes.Add(2, evento);
	}


	public void AddBeginInputEvent(UnityEngine.KeyCode Input, Action Evento)
	{
		controller.keys.Add(Input);
		controller.OnBeginKeyCode.Add(Input, Evento);
	}
	public void AddContinuousInputEvent(UnityEngine.KeyCode Input, Action Evento)
	{
		controller.keys.Add(Input);
		controller.OnKeyCode.Add(Input, Evento);
	}
	public void AddReleaseInputEvent(UnityEngine.KeyCode Input, Action Evento)
	{
		controller.keys.Add(Input);
		controller.OnKeyCode.Add(Input, Evento);
	}

	public void AddMouseEvent(int mouseButton, Action Evento)
	{
		controller.OnMouse.Add(mouseButton, Evento);
	}
	public void AddOnBeginMouseEvent(int mouseButton, Action Evento)
	{
		//Añado el evento a un Dictionary.
		controller.OnBeginMouse.Add(mouseButton, Evento);
	}
	public void AddOnEndMouseEvent(int mouseButton, Action Evento)
	{
		controller.OnReleaseMouse.Add(mouseButton, Evento);
	}
	#endregion
}

