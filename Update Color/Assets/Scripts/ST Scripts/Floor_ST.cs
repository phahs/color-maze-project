using UnityEngine;
using System.Collections;

public class Floor_ST : Tile_ST
{
    private Tile_ST[] surroundings = new Tile_ST[8];
    // 7   0   1
    // 6   me  2
    // 5   4   3

    public override void Start()
    {

    }

    public void setIndex(int x, int z)
    {
        mapIndex = new Vector3(x, 0, z);
    }

    public void addSurroundings(Tile_ST[,] originalMap, int x, int z, Tile_ST wallPrefab, Room_ST room)
    {
        mapIndex = new Vector3(x, 0, z);
        Tile_ST[] adjTiles = sweep(originalMap, x, z, wallPrefab, room);

        for (int k = 0; k < surroundings.Length; k++)
        {
            surroundings[k] = adjTiles[k];
            if (surroundings[k].tag == "Floor")
            {
                surroundings[k].addTile(this);
            }
        }
    }

    public override void addTile(Floor_ST other)
    {
        Vector3 distance = other.transform.position - transform.position;

        int index = determineLocation(distance);

        surroundings[index] = other;
    }

    private Tile_ST[] sweep(Tile_ST[,] oMap, int x, int z, Tile_ST wallPrefab, Room_ST room)
    {
        Tile_ST[] nearTiles = new Tile_ST[8];

        for (int i = 0; i < nearTiles.Length; i++)
        {
            nearTiles[i] = findTile(oMap, x, z, i, wallPrefab, room);
        }

        return nearTiles;
    }

    private Tile_ST findTile(Tile_ST[,] tempMap, int x, int z, int index, Tile_ST wallPrefab, Room_ST room)
    {
        Tile_ST aTile = null;
        int tempx = 0;
        int tempz = 0;
        int size = 6;

        switch (index)
        {
            case 0:
                tempx = x;
                tempz = z + 1;
                aTile = tempMap[tempx, tempz];
                break;
            case 1:
                tempx = x + 1;
                tempz = z + 1;
                aTile = tempMap[tempx, tempz];
                break;
            case 2:
                tempx = x + 1;
                tempz = z;
                aTile = tempMap[x + 1, tempz];
                break;
            case 3:
                tempx = x + 1;
                tempz = z - 1;
                aTile = tempMap[tempx, tempz];
                break;
            case 4:
                tempx = x;
                tempz = z - 1;
                aTile = tempMap[tempx, tempz];
                break;
            case 5:
                tempx = x - 1;
                tempz = z - 1;
                aTile = tempMap[tempx, tempz];
                break;
            case 6:
                tempx = x - 1;
                tempz = z;
                aTile = tempMap[tempx, tempz];
                break;
            case 7:
                tempx = x - 1;
                tempz = z + 1;
                aTile = tempMap[tempx, tempz];
                break;
        }

        if (aTile == null)
        {
            aTile = Instantiate(wallPrefab) as Wall_ST;
            tempMap[tempx, tempz] = aTile;
            aTile.name = "Wall " + tempx + ", " + tempz;
            aTile.transform.parent = room.transform;
            aTile.transform.localPosition = new Vector3(tempx - size * 0.5f + 0.5f, 0, tempz - size * 0.5f + 0.5f);
        }

        return aTile;
    }

    private int determineLocation(Vector3 dist)
    {
        int index = 0;

        if (dist.x == 0)
        {
            if (dist.z == 1)
            {
                index = 0;
            }
            else if (dist.z == -1)
            {
                index = 4;
            }
        }
        else if (dist.x < 0)
        {
            if (dist.z == 0)
            {
                index = 6;
            }
            else if (dist.z < 0)
            {
                index = 5;
            }
            else if (dist.z > 0)
            {
                index = 7;
            }
        }
        else if (dist.x < 0)
        {
            if (dist.z == 0)
            {
                index = 2;
            }
            else if (dist.z < 0)
            {
                index = 3;
            }
            else if (dist.z > 0)
            {
                index = 1;
            }
        }
        return index;
    }
}
