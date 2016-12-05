using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    private Coordinates areaStartPoint;
    private BoxCollider box;
    private Vector2 areaCenter;
    private int xValue, zValue;

    public void setCoord(Coordinates coord, int numRooms, int sizeX, int sizeZ)
    {
        areaStartPoint = new Coordinates(coord.x, coord.z);
        determineRoomArea(numRooms, sizeX, sizeZ);
        determineCenter(numRooms);
        box = GetComponent<BoxCollider>();
        box.center = new Vector3(areaCenter.x, 0, areaCenter.y);
        box.size = new Vector3(numRooms, 0, numRooms);
    }

    public int getXCoord()
    {
        return areaStartPoint.x;
    }

    public int getZCoord()
    {
        return areaStartPoint.z;
    }

    public Coordinates getCoord()
    {
        return areaStartPoint;
    }

    public int getXBound()
    {
        return xValue;
    }

    public int getZBound()
    {
        return zValue;
    }

    public Vector2 getCenter()
    {
        return areaCenter;
    }

    public bool compareCenter(Room other)
    {
        Vector2 temp = other.getCenter();

        if(areaCenter == temp)
        {
            return false;
        }

        return true;
    }

    private void determineRoomArea(int numRooms, int sizeX, int sizeZ)
    {
        if (getXCoord() + numRooms >= sizeX)
        {
            xValue = getXCoord() - numRooms;
        }
        else
        {
            xValue = getXCoord() + numRooms;
        }

        if (getZCoord() + numRooms >= sizeZ)
        {
            zValue = getZCoord() - numRooms;
        }
        else
        {
            zValue = getZCoord() + numRooms;
        }
    }

    private void determineCenter(int length)
    {
        float xCenter = Mathf.Min(xValue, areaStartPoint.x) + length * 0.5f;
        float zCenter = Mathf.Min(zValue, areaStartPoint.z) + length * 0.5f;

        xCenter -= length * 5;
        zCenter -= length * 5;

        areaCenter = new Vector2(xCenter, zCenter);
    }
}
