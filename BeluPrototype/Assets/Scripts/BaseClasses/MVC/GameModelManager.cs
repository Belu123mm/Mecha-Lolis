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
	/// <summary>
	/// Añade un evento de teclado que responde al Axis "Horizontal".
	/// </summary>
	/// <param name="type">1 = OnBegin, 2 = Continuous, 3 = OnRelease.</param>
	/// <param name="evento">Evento a ejecutar.</param>
	public void AddHorizontalEvent(InputEventType type, Action<float> evento)
	{
		switch (type)
		{
			case InputEventType.OnBegin:
				if (controller.OnBeginAxes.ContainsKey(1)) controller.OnBeginAxes[1] += evento;
				else controller.OnBeginAxes.Add(1, evento);
				break;
			case InputEventType.Continious:
				if (!controller.OnGetAxes.ContainsKey(1)) controller.OnGetAxes.Add(1, evento);
				else controller.OnGetAxes[1] += evento;
				break;
			case InputEventType.OnRelease:
				if (!controller.OnReleaseAxes.ContainsKey(1)) controller.OnReleaseAxes.Add(1, evento);
				else controller.OnReleaseAxes[1] += evento;
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Añade un evento de teclado que responde al Axis "Vertical".
	/// </summary>
	/// <param name="type">1 = OnBegin, 2 = Continuous, 3 = OnRelease.</param>
	/// <param name="evento">Evento a ejecutar.</param>
	public void AddVerticalEvent(InputEventType type, Action<float> evento)
	{
		switch (type)
		{
			case InputEventType.OnBegin:
				if (!controller.OnBeginAxes.ContainsKey(2)) controller.OnGetAxes.Add(2, evento);
				else controller.OnGetAxes[2] += evento;
				break;
			case InputEventType.Continious:
				if (!controller.OnGetAxes.ContainsKey(2)) controller.OnGetAxes.Add(2, evento);
				else controller.OnGetAxes[2] += evento;
				break;
			case InputEventType.OnRelease:
				if (!controller.OnReleaseAxes.ContainsKey(2)) controller.OnReleaseAxes.Add(2, evento);
				else controller.OnReleaseAxes[2] += evento;
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
		switch (type)
		{
			case InputEventType.OnBegin:
				controller.keys.Add(Input);
				controller.OnBeginKeyCode.Add(Input, Evento);
				break;
			case InputEventType.Continious:
				controller.keys.Add(Input);
				controller.OnKeyCode.Add(Input, Evento);
				break;
			case InputEventType.OnRelease:
				controller.keys.Add(Input);
				controller.OnKeyCode.Add(Input, Evento);
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
				controller.OnBeginMouse.Add(mouseButton, Evento);
				break;
			case InputEventType.Continious:
				controller.OnMouse.Add(mouseButton, Evento);
				break;
			case InputEventType.OnRelease:
				controller.OnReleaseMouse.Add(mouseButton, Evento);
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
		controller.MouseAxisTrack += MouseTrack;
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
