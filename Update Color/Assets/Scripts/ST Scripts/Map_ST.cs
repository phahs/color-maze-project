using UnityEngine;
using System.Collections;

public class Map_ST : MonoBehaviour
{
    public Room_ST roomPrefab;
    public Tile_ST floorPrefab;
    public Tile_ST wallPrefab;
    public GameObject wallList;
    public GameObject floorList;

    private Tile_ST[] allTiles; // Do I actually need this?
    private Tile_ST[,] map;
    private int size;

    public void createMap()
    {
        size = 6;
        // int totalTiles = size * size;

        organizeHeirarchy();
        setMap(size);

        buildFloor(size);

        printTileNames();

    }

    public Tile_ST[,] getMap()
    {
        return map;
    }

    private void organizeHeirarchy()
    {
        wallList = new GameObject();

        wallList.transform.parent = transform;
        wallList.transform.localPosition = transform.localPosition;
        wallList.name = "Wall List";

        floorList = new GameObject();

        floorList.transform.parent = transform;
        floorList.transform.localPosition = transform.localPosition;
        floorList.name = "Floor List";
    }

    private void setMap(int size)
    {
        map = new Tile_ST[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                map[x, z] = null;
            }
        }
    }

    private void buildFloor(int size)
    {
        for (int x = 1; x < size - 1; x++)
        {
            for (int z = 1; z < size - 1; z++)
            {
                Floor_ST newFloor = Instantiate(floorPrefab) as Floor_ST;
                if (map[x, z] == null)
                {
                    map[x, z] = newFloor;
                }
                else
                {
                    Destroy(map[x, z].gameObject);
                    map[x, z] = newFloor;
                }
                newFloor.transform.parent = floorList.transform;
                newFloor.transform.localPosition = new Vector3(x - size * 0.5f + 0.5f, 0, z - size * 0.5f + 0.5f);
                newFloor.name = "Floor " + x + ", " + z;
                newFloor.addSurroundings(map, x, z, wallPrefab, wallList);
            }
        }
    }

    private void printTileNames()
    {
        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k < size; k++)
            {
                Debug.Log(map[i, k].name);
            }
        }
    }
}
