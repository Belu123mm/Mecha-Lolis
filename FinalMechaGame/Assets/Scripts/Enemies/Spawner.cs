using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public List<Wave> waves;
	public List<NodeGroup> nodeGroups;
	public float waitToStartTime;
	public float timer;
	public bool start;
	public bool finished;
	public Transform target;
	public float timeBetweenWaves;

	private void Start() {
		foreach ( var w in waves ) {
			if ( !w.hasNodes )
				if ( w.positions.Count == 0 )
					w.positions.Add(new Positions(Vector3.zero,false));
		}
	}

	void Update() {
		timer += Time.deltaTime;
		if ( timer > waitToStartTime && !finished ) {
			start = true;
			timer = 0;
		}
		if ( start ) {
			StartCoroutine(GenerateEnemy());
			start = false;
			finished = true;
		}
		//Hasta aqui todo perfecto
	}
	IEnumerator GenerateEnemy() {
		foreach ( var w in waves ) {
			for ( int i = 0; i < w.qt; i++ ) {
				Turret en = w.enemy;
				en.target = target;
				if ( w.hasNodes )
					en.nodegroup = nodeGroups [ Random.Range(0,nodeGroups.Count) ];
				if ( en.hasNavigation )
					Instantiate(en, en.nodegroup._first.position, Quaternion.identity, this.transform);
                else {
                    Instantiate(en, GetPositionToSpawn(w,0), Quaternion.identity,this.transform);
                }
				yield return new WaitForSeconds(w.spawntime);
			}
			yield return new WaitForSeconds(timeBetweenWaves);
		}

	}
    Vector3 GetPositionToSpawn(Wave w,int index) {
        if(w.positions[index].visited == true ) {
            return GetPositionToSpawn(w, index + 1);
        }
        w.positions[ index ].visited = true;
        return w.positions [ index ].position;
    }

	public void OnDrawGizmos() {
		foreach ( var w in waves ) {
			foreach ( var n in w.positions ) {
				Gizmos.DrawCube(n.position, new Vector3(1, 1, 1));
			}
		}
	}
	//asi funciona pero sin timers
	//-dab-
	//Alan <3
}
