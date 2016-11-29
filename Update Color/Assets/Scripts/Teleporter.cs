using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Coordinates destination;
    public Vector2 dest;

    public void OnTriggerEnter(Collider other)
    {
        // screen flash to color of level
        // do the teleport while screen is the color
       
    }

    public Coordinates getDestination()
    {
        return destination;
    }

    public void setDestination(Map_Tile tile)
    {
        destination = tile.coord;
        dest.x = tile.coord.x;
        dest.y = tile.coord.z;
    }
}