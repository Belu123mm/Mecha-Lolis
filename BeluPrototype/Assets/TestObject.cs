using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour {
    [Range(0,1)]public float value;

    // Update is called once per frame
    void Update ()
    {
        GameManagerView.instance. = value.ToString();
	}
}
