using System.Collections.Generic;
[System.Serializable]
public class Wave {
    public Turret enemy;
    public float qt;
    public float spawntime;
    public bool hasNodes;
    public List<UnityEngine.Vector3> positions;
}
