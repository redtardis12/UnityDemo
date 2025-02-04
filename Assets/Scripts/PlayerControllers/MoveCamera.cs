using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform player;
    public Vector3 offset = new Vector3(0, 1, -1.5f);

    void Update() {
       transform.position = player.transform.position + offset;
    }
}