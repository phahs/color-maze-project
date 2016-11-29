using UnityEngine;
using System.Collections;

public class Map_Tile : MonoBehaviour
{
    public int tileType;
    public Coordinates coord;

    public Map_Tile()
    {
        coord = new Coordinates();
    }

    public Map_Tile(int x, int z)
    {
        coord = new Coordinates(x, z);
    }

    public void setCoordinates(Coordinates coord)
    {
        this.coord = coord;
    }

    public void setCoordinates(int x, int z)
    {
        coord.x = x;
        coord.z = z;
    }

    public Coordinates getCoordinates()
    {
        return coord;
    }

    public int getXCoordinates()
    {
        return coord.x;
    }

    public int getZCoordinates()
    {
        return coord.z;
    }
}