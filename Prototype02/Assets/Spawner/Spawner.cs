using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public List<Wave> oleadas;
    public List<NodeGroup> grupos;
    public Wave currentWave;
    public float waitToStartTime;
    public float cooldown;
    public float timer;
    public bool start;
    public int count;
    public int waveCount;
    public bool cooldowning;

    public void Start() {
        waveCount = 0;
        if ( oleadas.Count > waveCount ) currentWave = oleadas [ waveCount ];
    }
    public void Update() {
    }
}