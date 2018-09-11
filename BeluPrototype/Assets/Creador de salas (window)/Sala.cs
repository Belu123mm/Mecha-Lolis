using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sala : MonoBehaviour {
    public Material floor;
    public Material walls;
    public Material door;
    public string roomName;
    public RoomType type;
}

public enum RoomType {
    living,
    bedroom,
    bathroom
}
