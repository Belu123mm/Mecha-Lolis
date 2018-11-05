using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Esta clase maneja datos nativos. Clases y Operaciones del juego.
/// </summary>
public class GameModelManager : MonoBehaviour
{
	public static GameModelManager instance;
	public List<GameObject> PlayerList;
	public Pool<GameObject> PlayerBulletPool;
	public Pool<GameObject> EnemyBulletPool;
	[Header("Bullet Pools")]
	public GameObject bulletParent;
	public GameObject bulletPrefab;
	public GameObject enemyBulletPrefab;
	[Header("Game Stat")]
	public float Points = 0;
	public bool Paused = false;

	GameManagerView _gameView;
	SceneManagement _scenes;

	//Tiempo de juego.
	//Recuento de oleadas.
	//Lista de Spawners.(?).
	//Lista de enemigos. (?).
	//public static List<Player> Player; //PlayerSerializable.

	private void Awake()
	{
		if (instance == null) instance = this;
		_gameView = FindObjectOfType<GameManagerView>();
		_scenes = FindObjectOfType<SceneManagement>();
		UpdatePoints();
		InitializePlayerBulletPool(50, bulletPrefab, true);
		InitializeEnemyBulletPool(60, enemyBulletPrefab, true);
	}

	public void InitializeEnemyBulletPool(int ammount,GameObject bulletPrefab,bool isDinamic = false)
	{
		//Factory
		Func<GameObject> factory = () => 
		{ return Instantiate(
			bulletPrefab, 
			Vector3.zero, 
			Quaternion.identity,
			bulletParent.transform);
		};

		//Bullet Pool
		EnemyBulletPool = new Pool<GameObject>(
			ammount,
			factory,
			  EnemyBullet.InitializeBullet,
			  EnemyBullet.DeactivateBullet,
			isDinamic);

		//Método de reemplazo para Destroy()
		EnemyBullet.OnDeactivate = EnemyBulletPool.ReturnObjectToPool;
	}
	public void InitializePlayerBulletPool(int Ammount, GameObject bulletPrefab, bool CanExpand)
	{
		//Factory
		Func<GameObject> factory = () =>
		{
			return Instantiate(
			  bulletPrefab,
			  Vector3.zero,
			  Quaternion.identity,
			  bulletParent.transform);
		};

		// *Create the Bullet Pool
		PlayerBulletPool = new Pool<GameObject>(
			Ammount,
			factory,
			Bullet.InitializeBullet,
			Bullet.DeactivateBullet,
			CanExpand);

		//Método de reemplazo para Destroy()
		Bullet.OnDeactivate = PlayerBulletPool.ReturnObjectToPool;
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

	public void AdviceReload(bool show)
	{
		if (_gameView.LowAmmoAdvice.activeSelf) _gameView.LowAmmoAdvice.SetActive(false);
		_gameView.ReloadAdvice.SetActive(show);
	}
	public void AdviceLowAmmo(bool show)
	{
		_gameView.LowAmmoAdvice.SetActive(true);
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
	public void Pause()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;

		foreach (var Player in PlayerList)
		{
			Player.GetComponent<InputController>().enabled = false;
		}

		//Esto no va a funcionar con el player.
		Time.timeScale = 0;
		_gameView.PauseMenu.SetActive(true);
	}
	public void Continue()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		foreach (var Player in PlayerList)
		{
			Player.GetComponent<InputController>().enabled = true;
		}

		//Esto no va a funcionar con el player.
		Time.timeScale = 1;
		_gameView.PauseMenu.SetActive(false);
	}
}
