using UnityEngine;

public class LifeBarrController : MonoBehaviour {
    public RectTransform BackGround;
    public RectTransform Front;
    /// <summary>
    /// El valor debe estar entre 0 y 1. Siendo 1 equivalente al 100%.
    /// </summary>
    public float HPDisplay { set { Front.localScale = new Vector3(value, 1, 1); } }
}
