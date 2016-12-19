using UnityEngine;
using System.Collections;

public class Room_ST : MonoBehaviour
{
    Room_ST[] otherRooms;
    Tile_ST[] allTiles;

    Vector3 corner1, corner3, areaCenter;
    BoxCollider box;
    // Potential area map
    // 3   4   3
    // 2   1   2
    // 3   4   3

    public void setArea(Vector3 c1, int roomSize, int mapSize)
    {
        box = GetComponent<BoxCollider>();
        corner1 = new Vector3(c1.x, c1.y, c1.z);
        determineCorner3(roomSize, mapSize);
        determineCenter(roomSize);
        box.center = areaCenter;
        box.size = new Vector3(roomSize + 1, 0, roomSize + 1);
    }

    public Vector3 getCenter()
    {
        return areaCenter;
    }

    public float getC1X()
    {
        return corner1.x;
    }

    public float getC1Z()
    {
        return corner1.z;
    }

    public float getC3X()
    {
        return corner3.x;
    }

    public float getC3Z()
    {
        return corner3.z;
    }

    private void determineCorner3(int roomSize, int mapSize)
    {
        int c3X, c3Z;

        if (corner1.x + roomSize >= mapSize - 1)
        {
            c3X = (int)corner1.x - roomSize;
        }
        else
        {
            c3X = (int)corner1.x + roomSize;
        }

        if (corner1.z + roomSize >= mapSize - 1)
        {
            c3Z = (int)corner1.z - roomSize;
        }
        else
        {
            c3Z = (int)corner1.z + roomSize;
        }

        corner3 = new Vector3(c3X, 0, c3Z);
    }

    private void determineCenter(int roomSize)
    {
        float xCenter = Mathf.Min(corner1.x, corner3.x) + roomSize * 0.5f;
        float zCenter = Mathf.Min(corner1.z, corner3.z) + roomSize * 0.5f;

        xCenter -= roomSize * 5;
        zCenter -= roomSize * 5;

        areaCenter = new Vector3(xCenter, 0, zCenter);
    }
}

