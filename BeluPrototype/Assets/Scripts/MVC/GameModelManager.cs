//using System.Collections;
//using System.Collections.Generic;
//Al ser estática está desde el principio del programa, no necesita Herencias de Monobehaviour.
//Se encuentra en namespace::global. Cualquier clase puede acceder a ella.
//Es un objeto que guarda el estado del juego en memoria.

using System;
/// <summary>
/// Esta clase maneja datos nativos. Clases y Operaciones.
/// </summary>
public static class GameModelManager
{
    public static float Points = 0;
    public static GameManagerView gameView = new GameManagerView();
    //Tiempo de juego.
    //Recuento de oleadas.
    //Lista de Spawners.(?).
    //Lista de enemigos. (?).
    //public static List<Player> Player; //PlayerSerializable.

    /// <summary>
    /// Inicia el juego.
    /// </summary>
    public static void StartGame()
    {
        //Acá iría todo lo que se hace cuando el juego Empieza.
        //Si el juego comienza desde cero no cargamos nada, pero si el jugador lo desea puede cargar una partida en cualquier momento.
    }

    /// <summary>
    /// Termina el juego.
    /// </summary>
    public static void EndGame()
    {
        //Acá iría todo lo que se hace cuando el juego termina.
        //aca tendriamos una instancia de "SceneManager" por ejemplo y ejecutar su función.
        //Por ejemplo, decirle a nuestro view que habra la pantalla de derrota. Dejandonos cargar la ultima partida.
    }

    public static void LoadGame()
    {
        //Des-Serializa el estado del juego y lo carga en memoria.
    }

    public static void SaveGame()
    {
        //Serializa el estado del juego y lo guarda en disco.
    }

    public static void HPUpdate(float value)
    {

    }
}

