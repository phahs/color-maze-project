using UnityEngine;
using System.Collections;

public class Room_ST : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void findPathRoom(Vector3 currentLocal, Vector3 destination)
    {
        // NPC needs to know what room they are in.
        // Room needs to know what teleporters are in the room and where they go
        // Given that NPC is in room X, determine what room destination is in
        // If destination is in room X, call findPathFloor(currentLocal, destination)
        // Else If destination is not in room X query teleporters for their dest
        // Ask teleporter dest Rooms if they hold destination
        // If it does, have teleporter dest Room call findPathFloor(teleporterDest, destination) while room X calls findPathFloor(currentLocal, teleporterLocal)
        // Else If neither do, repeat from line 25 until destination is found
    }
}

