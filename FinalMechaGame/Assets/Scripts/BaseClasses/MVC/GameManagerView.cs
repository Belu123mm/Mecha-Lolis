using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Esta clase Maneja toda la UI del juego. No deberia hacer cálculo de valores.
/// </summary>
public class GameManagerView : MonoBehaviour {
    public LifeBarrController barraDeVida;
    public Text Points_T;
    [SerializeField]
    protected Text Bullets;
    [SerializeField]
    protected Text MaxBullets;
    public GameObject ReloadAdvice;
    public GameObject LowAmmoAdvice;
    public GameObject PauseMenu;

    public string PointDisplay { set { Points_T.text = value; } }
    public float LifeDisplay { set { barraDeVida.HPDisplay = value; } }
    public string BulletDisplay { set { Bullets.text = value; } }
    public string MaxBulletDisplay { get { return MaxBullets.text; }  set { MaxBullets.text = value; } }
}
