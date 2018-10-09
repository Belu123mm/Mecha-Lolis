using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Wave {
    public Dictionary<Enemy, int> enemiesDic;
    public float timeBetweenEnemies;
    public UnityEngine.Vector3 positionToSpawn;
}