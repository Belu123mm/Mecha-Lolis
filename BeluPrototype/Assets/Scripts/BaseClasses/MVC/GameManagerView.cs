using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Esta clase Maneja toda la UI del juego. No deberia hacer cálculo de valores.
/// </summary>
public class GameManagerView : MonoBehaviour {
    public LifeBarrController barraDeVida;
    public Text Points_T;

    public string PointDisplay { set { Points_T.text = value; } }
    public float LifeDisplay { set { barraDeVida.HPDisplay = value; } }
}
