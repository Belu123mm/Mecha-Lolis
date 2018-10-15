using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float life;
    public float size;

    private void Start()
    {
        this.transform.localScale = new Vector3(size,size,size);
    }
}
