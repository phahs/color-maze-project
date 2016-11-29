using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Map_Tile destination;
    public Vector2 dest;
    public Canvas playerUI;

    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //turn screen white + color of level
            Time.timeScale = 0;
            other.transform.position = new Vector3(destination.transform.position.x, 0, destination.transform.position.z);
            Time.timeScale = 1;
        }
        else
        {
            // local flash at both teleporter location and destination?
            other.transform.position = new Vector3(destination.transform.position.x, 0, destination.transform.position.z);
        }
       
    }

    public Map_Tile getDestination()
    {
        return destination;
    }

    public void setDestination(Map_Tile tile)
    {
        destination = tile;
        dest.x = tile.transform.position.x;
        dest.y = tile.transform.position.z;
    }
}