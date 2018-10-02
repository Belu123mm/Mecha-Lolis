using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {
    //Esta es la clase en donde se maneja absolutamente todo lo visual
    //En este caso voy a hacer lo de los materiales
    //Aunque he probado con animaciones, modelos, meshes, texturas, etcetcetcetcetc

    public Material green;
    public Material blue;
    public MeshRenderer mr;

    //Lo basico del start/avake para declarar variables 
	void Start () {
        ChangeMaterial(green);
    }

    //Funciones que tienen que ver con la vision 
    public void ChangeMaterial(Material m ) {
        mr.material = m;
    }

}
