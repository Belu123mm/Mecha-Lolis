using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
    public Turret enemy;
    public float qt;
    public float spawntime;
    public bool hasNodes;
    public List<Positions> positions;
}
[System.Serializable]
public class Positions {
    public Vector3 position;
    public bool visited;

    public Positions( Vector3 position, bool visited ) {
        this.position = position;
        this.visited = visited;
    }
}

