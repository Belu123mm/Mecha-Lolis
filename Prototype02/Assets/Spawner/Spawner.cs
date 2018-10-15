using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public List<Wave> oleadas;
    public List<NodeGroup> nodeGroups;
    public float waitToStartTime;
    public float timer;
    public bool start;
    public bool finished;
    public Transform target;
    public float timeBetweenWaves;

    void Update() {
        timer += Time.deltaTime;
        if ( timer > waitToStartTime && !finished ) {
            start = true;
        }
        if ( start ) {
            StartCoroutine(GenerateEnemy());
            start = false;
            finished = true;
        }
        //Hasta aqui todo perfecto
    }
    IEnumerator GenerateEnemy() {
        foreach ( var w in oleadas ) {
            for ( int i = 0; i < w.qt; i++ ) {
                print(i);
                Turret en = w.enemy;
                en.target = target;
                if ( w.hasNodes )
                    en.nodegroup = nodeGroups [ 0 ];
                Instantiate(en);
                yield return new WaitForSeconds(w.spawntime);
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }

    }
    //asi funciona pero sin timers
    //-dab-
    //Alan <3
}

