using UnityEngine;
using System.Collections;

public class Map_ST : MonoBehaviour
{
    public Room_ST roomPrefab;
    public Tile_ST floorPrefab;
    public Tile_ST wallPrefab;
    public Tile_ST obstaclePrefab;
    public Tile_ST teleporterPrefab;
    
    private Tile_ST[,] map;
    private Room_ST[] rooms;
    private BoxCollider box;
    private int size;
    private int baseRoomNum = 6;
    private int totalRoomNum;

    public void createMap(int level)
    {
        defineMapArray(level);

        defineMapObjects();
    }

    private void defineMapArray(int level)
    {
        box = GetComponent<BoxCollider>();
        totalRoomNum = baseRoomNum + level;
        size = totalRoomNum * 10;
        box.size = new Vector3(size, 0, size);
        setMap(size);
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

    private void defineMapObjects()
    {
        defineRooms();

        for(int i = 0; i < rooms.Length; i++)
        {
            createFloors(rooms[i]);
        }

        for(int j = 0; j < rooms.Length; j++)
        {
            createWalls(rooms[j]);
        }
        /*
        for(int q = 0; q < rooms.Length; q++)
        {
            linkKnowledge(rooms[q];
        }*/
    }

    private void defineRooms()
    {
        rooms = new Room_ST[totalRoomNum];

        for(int w = 0; w < totalRoomNum; w++)
        {
            rooms[w] = createRoom(totalRoomNum, w + 1);
        }
    }

    private Room_ST createRoom(int roomSize, int roomNumber)
    {
        Room_ST newRoom = Instantiate(roomPrefab) as Room_ST;
        newRoom.transform.parent = transform;
        newRoom.name = "Room " + roomNumber;
        minimizeOverlap(newRoom, roomSize);

        return newRoom;
    }
    
    private void minimizeOverlap(Room_ST room, int roomSize)
    {
        Room_ST[] allCreatedRooms = GetComponentsInChildren<Room_ST>();
        Vector3 areaCorner = new Vector3(Random.Range(1, size), 0, Random.Range(1, size));
        bool allClear = false;
        int index = 0;

        room.setArea(areaCorner, roomSize, size);
        
        while(!allClear)
        {
            if (allCreatedRooms[index].name == room.name)
            {
                allClear = true;
            }

            if(!allClear)
            {
                float centerOffset = roomSize * 0.5f;
                Vector3 currentRoomCenter = room.getCenter();
                Vector3 testRoomCenter = allCreatedRooms[index].getCenter();
                Vector3 vector = testRoomCenter - currentRoomCenter;

                if(Mathf.Abs(vector.magnitude) <= centerOffset)
                {
                    areaCorner = new Vector3(Random.Range(1, roomSize), 0, Random.Range(1, roomSize));
                    room.setArea(areaCorner, roomSize, size);
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
        }
    }

    private void createFloors(Room_ST room)
    {
        int availableTiles = totalRoomNum * totalRoomNum;
        int tilesToBeUsed = Random.Range(Mathf.RoundToInt(availableTiles * 0.5f), Mathf.RoundToInt(availableTiles * 0.75f));

        Vector3 newTileCoord = new Vector3(Random.Range(Mathf.Min((int)room.getC1X(), (int)room.getC3X()), Mathf.Max((int)room.getC1X(), (int)room.getC3X())), 0, Random.Range(Mathf.Min((int)room.getC1Z(), (int)room.getC3Z()), Mathf.Max((int)room.getC1Z(), (int)room.getC3Z())));

        while (map[(int)newTileCoord.x, (int)newTileCoord.z] != null)
        {
            newTileCoord = Vector3.zero;
            newTileCoord = new Vector3(Random.Range(Mathf.Min((int)room.getC1X(), (int)room.getC3X()), Mathf.Max((int)room.getC1X(), (int)room.getC3X())), 0, Random.Range(Mathf.Min((int)room.getC1Z(), (int)room.getC3Z()), Mathf.Max((int)room.getC1Z(), (int)room.getC3Z())));
        }

        Vector3 coordCheck = new Vector3(newTileCoord.x, newTileCoord.y, newTileCoord.z);
        int index = 0;

        while(index < tilesToBeUsed)
        {
            makeFloorTile(newTileCoord, room);
            newTileCoord = chooseNextTile(newTileCoord, room);

            if(coordCheck.x == newTileCoord.x && coordCheck.z == newTileCoord.z)
            {
                Tile_ST[] floors = room.GetComponentsInChildren<Floor_ST>();
                if (floors.Length == 1)
                {
                    Destroy(floors[0].gameObject);
                    newTileCoord = new Vector3(Random.Range(Mathf.Min((int)room.getC1X(), (int)room.getC3X()), Mathf.Max((int)room.getC1X(), (int)room.getC3X())), 0, Random.Range(Mathf.Min((int)room.getC1Z(), (int)room.getC3Z()), Mathf.Max((int)room.getC1Z(), (int)room.getC3Z())));

                    while (map[(int)newTileCoord.x, (int)newTileCoord.z] != null)
                    {
                        newTileCoord = Vector3.zero;
                        newTileCoord = new Vector3(Random.Range(Mathf.Min((int)room.getC1X(), (int)room.getC3X()), Mathf.Max((int)room.getC1X(), (int)room.getC3X())), 0, Random.Range(Mathf.Min((int)room.getC1Z(), (int)room.getC3Z()), Mathf.Max((int)room.getC1Z(), (int)room.getC3Z())));
                    }
                    index = -1;
                }
                else
                {
                    break;
                }
            }

            coordCheck.x = newTileCoord.x;
            coordCheck.z = newTileCoord.z;

            index += 1;
        }

    }

    private void makeFloorTile(Vector3 mapCoordFloat, Room_ST room)
    {
        Floor_ST newFloor = Instantiate(floorPrefab) as Floor_ST;
        map[(int)mapCoordFloat.x, (int)mapCoordFloat.z] = newFloor;
        newFloor.name = "Floor " + mapCoordFloat.x + ", " + mapCoordFloat.z;
        newFloor.setIndex((int)mapCoordFloat.x, (int)mapCoordFloat.z);
        newFloor.transform.parent = room.transform;
        newFloor.transform.localPosition = new Vector3(mapCoordFloat.x - size * 0.5f + 0.5f, 0, mapCoordFloat.z - size * 0.5f + 0.5f);
    }

    private Vector3 chooseNextTile(Vector3 mapCoordFloat, Room_ST room)
    {
        int rand;
        int[] choices1 = { 1, 2, 3, 4 };
        Vector3 nextCoord = new Vector3(mapCoordFloat.x, mapCoordFloat.y, mapCoordFloat.z);

        for (int w = 0; w < choices1.Length; w++)
        {
            rand = Random.Range(w, choices1.Length);

            if (validTile(choices1[rand], ref nextCoord, room))
            {
                return nextCoord;
            }
            choices1[rand] = choices1[w];
        }

        Tile_ST[] floorTiles = room.GetComponentsInChildren<Floor_ST>();

        for(int j = 0; j < floorTiles.Length; j++)
        {
            int[] choices2 = { 1, 2, 3, 4 };
            nextCoord = floorTiles[j].getIndex();

            for (int k = 0; k < choices2.Length; k++)
            {
                if (validTile(choices2[k], ref nextCoord, room))
                {
                    return nextCoord;
                }
            }
        }

        return mapCoordFloat;
    }

    private bool validTile(int direction, ref Vector3 coord, Room_ST room)
    {
        Vector3 tempCoord = new Vector3(coord.x, coord.y, coord.z);
        switch(direction)
        {
            case 1:
                // North: +z
                tempCoord.z += 1;
                if ((int)tempCoord.z < Mathf.Max((int)room.getC1Z(), (int)room.getC3Z()))
                {
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null)
                    {
                        coord.z = tempCoord.z;
                        return true;
                    }
                }
                break;
            case 2:
                // East: +x
                tempCoord.x += 1;
                if ((int)tempCoord.x < Mathf.Max((int)room.getC1X(), (int)room.getC3X()))
                {
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null)
                    {
                        coord.x = tempCoord.x;
                        return true;
                    }
                }
                break;
            case 3:
                // South: -z
                tempCoord.z -= 1;
                if ((int)tempCoord.z > Mathf.Min((int)room.getC1Z(), (int)room.getC3Z()))
                {
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null)
                    {
                        coord.z = tempCoord.z;
                        return true;
                    }
                }
                break;
            case 4:
                // West: -x
                tempCoord.x -= 1;
                if ((int)tempCoord.x > Mathf.Min((int)room.getC1X(), (int)room.getC3X()))
                {
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null)
                    {
                        coord.x = tempCoord.x;
                        return true;
                    }
                }
                break;
        }

        return false;
    }

    private void createWalls(Room_ST room)
    {
        Tile_ST[] roomFloor = room.GetComponentsInChildren<Tile_ST>();
        for(int e = 0; e < roomFloor.Length; e++)
        {
            wallAround(roomFloor[e], room);
        }
    }

    private void wallAround(Tile_ST floor, Room_ST room)
    {
        for(int a = 0; a < 8; a++)
        {
            Vector3 index = floor.getIndex();

            if (!isFloor(a, ref index))
            {
                makeWallTile(index, room);
            }
        }
    }
    
    private bool isFloor(int counter, ref Vector3 indexFloat)
    {
        switch (counter)
        {
            case 0:
                indexFloat.z += 1;
                break;
            case 1:
                indexFloat.x += 1;
                indexFloat.z += 1;
                break;
            case 2:
                indexFloat.x += 1;
                break;
            case 3:
                indexFloat.x += 1;
                indexFloat.z -= 1;
                break;
            case 4:
                indexFloat.z -= 1;
                break;
            case 5:
                indexFloat.x -= 1;
                indexFloat.z -= 1;
                break;
            case 6:
                indexFloat.x -= 1;
                break;
            case 7:
                indexFloat.x -= 1;
                indexFloat.z += 1;
                break;
        }

        if(map[(int)indexFloat.x, (int)indexFloat.z] == null)
        {
            return false;
        }

        return true;
    }

    private void makeWallTile(Vector3 indexFloat, Room_ST room)
    {
        Wall_ST newWall = Instantiate(wallPrefab) as Wall_ST;
        map[(int)indexFloat.x, (int)indexFloat.z] = newWall;
        newWall.name = "Wall " + indexFloat.x + ", " + indexFloat.z;
        //newWall.setIndex((int)indexFloat.x, (int)indexFloat.z);
        newWall.transform.parent = room.transform;
        newWall.transform.localPosition = new Vector3(indexFloat.x - size * 0.5f + 0.5f, 0, indexFloat.z - size * 0.5f + 0.5f);
    }

    /*private void createRooms(int numOfRooms)
    {
        for(int i = 0; i < numOfRooms; i++)
        {
            Room_ST newRoom = Instantiate(roomPrefab) as Room_ST;
            newRoom.transform.parent = transform;
            newRoom.name = "Room " + (i + 1);
            minimizeOverlap(newRoom, numOfRooms);
            addTilesToRoom(newRoom);
        }

        Room_ST[] rooms = GetComponentsInChildren<Room_ST>();
        for(int o = 0; o < rooms.Length; o++)
        {
            rooms[o].addRooms(rooms);
        }

    }

    private void minimizeOverlap(Room_ST room, int numOfRooms)
    {
        Room_ST[] allRooms = GetComponentsInChildren<Room_ST>();
        Vector3 areaPT = new Vector3(Random.Range(1, size - 1), 0, Random.Range(1, size - 1));
        bool allClear = false;
        int index = 0;

        room.setArea(areaPT, size, numOfRooms);

        while(!allClear)
        {
            if (allRooms[index].name == room.name)
            {
                allClear = true;
            }

            if(!allClear)
            {
                float centerOffset = (numOfRooms - 1) * 0.5f;
                Vector3 currentCenter = room.getCenter();
                Vector3 testCenter = allRooms[index].getCenter();
                Vector3 distance = testCenter - currentCenter;

                if (Mathf.Abs(distance.magnitude) <= centerOffset)
                {
                    areaPT = new Vector3(Random.Range(1, size - 1), 0, Random.Range(1, size - 1));
                    room.setArea(areaPT, size, numOfRooms);
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
        }
    }

    private void addTilesToRoom(Room_ST room)
    {
        Vector3 pt1 = room.getCorner1();
        Vector3 pt2 = room.getCorner3();
        int totalAvailableTiles = (Mathf.Max((int)pt1.x, (int)pt2.x) - Mathf.Min((int)pt1.x, (int)pt2.x)) * (Mathf.Max((int)pt1.z, (int)pt2.z) - Mathf.Min((int)pt1.z, (int)pt2.z));
        int tilesInRoom = Random.Range(Mathf.RoundToInt(totalAvailableTiles * 0.5f), Mathf.RoundToInt(totalAvailableTiles * 0.75f) + 1);

        room.setNumTiles(tilesInRoom);

        Vector3 newCoord = new Vector3(Random.Range(Mathf.Min((int)pt1.x, (int)pt2.x), Mathf.Max((int)pt1.x, (int)pt2.x) + 1), 0, Random.Range(Mathf.Min((int)pt1.z, (int)pt2.z), Mathf.Max((int)pt1.z, (int)pt2.z)) + 1);
        
        while(map[Mathf.RoundToInt(newCoord.x), Mathf.RoundToInt(newCoord.z)] != null)
        {
            newCoord = Vector3.zero;
            newCoord = new Vector3((Random.Range(Mathf.Min(pt1.x, pt2.x), Mathf.Max(pt1.x, pt2.x) + 1)), 0, (Random.Range(Mathf.Min(pt1.z, pt2.z), Mathf.Max(pt1.z, pt2.z)) + 1));
        }
        
        Vector3 coordCheck = new Vector3(newCoord.x, newCoord.y, newCoord.z);
        Debug.Log("addTilesToRoom newCoord: " + newCoord + "\naddTilesToRoom coordCheck: " + coordCheck);

        int index = 0;

        while(index < tilesInRoom)
        {
            createTile(newCoord, room);
            newCoord = chooseNextTile(newCoord, room);
            Debug.Log("After chooseNextTile newCoord: " + newCoord + "\nAfter chooseNextTile coordCheck: " + coordCheck);
            if (coordCheck.x == newCoord.x && coordCheck.z == newCoord.z)
            {
                Tile_ST[] floor = room.GetComponentsInChildren<Floor_ST>();

                if(floor.Length == 1)
                {/*
                    Tile_ST[] wall = room.GetComponentsInChildren<Wall_ST>();
                    for(int w = 0; w < wall.Length; w++)
                    {
                        Destroy(wall[w].gameObject);
                    }
                    Debug.Log("try again");
                    Destroy(floor[0].gameObject);
                    index = -1;
                }
                else
                {
                    break;
                }    
            }

            coordCheck.x = newCoord.x;
            coordCheck.z = newCoord.z;

            index += 1;
        }
    }

    private void createTile(Vector3 coord, Room_ST room)
    {
        Floor_ST newFloor = Instantiate(floorPrefab) as Floor_ST;
        if (map[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.z)] == null)
        {
            map[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.z)] = newFloor;
        }
        else
        {
            Destroy(map[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.z)].gameObject);
            map[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.z)] = newFloor;
        }
        newFloor.transform.parent = room.transform;
        newFloor.transform.localPosition = new Vector3(coord.x - size * 0.5f + 0.5f, 0, coord.z - size * 0.5f + 0.5f);
        newFloor.name = "Floor " + coord.x + ", " + coord.z;
        //newFloor.addSurroundings(map, Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.z), wallPrefab, room);
    }

    private Vector3 chooseNextTile(Vector3 coord, Room_ST room)
    {
        int rand;
        int[] choices = { 1, 2, 3, 4 };
        Vector3 newCoord = new Vector3(coord.x, coord.y, coord.z);

        for (int q = 0; q < 4; q++)
        {
            rand = Random.Range(q, choices.Length);
            Debug.Log("direction = " + choices[rand]);

            if (validTile(choices[rand], newCoord, room))
            {
                return newCoord;
            }

            choices[rand] = choices[q];
            Debug.Log("Rand chooseNextTile iteration: " + q);
        }

        Tile_ST[] nextFloor = room.GetComponentsInChildren<Floor_ST>();
        for(int j = 0; j < nextFloor.Length; j++)
        {
            int[] choices2 = { 1, 2, 3, 4 };
            newCoord = nextFloor[j].getIndex();
            for (int k = 0; k < 4; k++)
            {
                if(validTile(choices2[k], newCoord, room))
                {
                    return newCoord;
                }
                Debug.Log("Set chooseNextTile iteration: " + k);
            }
        }

        return coord;
    }

    private bool validTile(int direction, Vector3 coord, Room_ST room)
    {
        Vector3 tempCoord = new Vector3(coord.x, coord.y, coord.z);
        Vector3 pt1 = room.getCorner1();
        Vector3 pt2 = room.getCorner3();

        switch (direction)
        {
            case 1:
                tempCoord.z += 1;
                if (tempCoord.z < Mathf.Max(pt1.z, pt2.z))
                {
                    Debug.Log("case 1");
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null || map[(int)tempCoord.x, (int)tempCoord.z].tag != "Floor")
                    {
                        Debug.Log("complete");
                        coord.z = tempCoord.z;
                        return true;
                    }
                }
                break;
            case 2:
                tempCoord.x += 1;
                if (tempCoord.x < Mathf.Max(pt1.x, pt2.x))
                {
                    Debug.Log("case 2");
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null || map[(int)tempCoord.x, (int)tempCoord.z].tag != "Floor")
                    {
                        Debug.Log("complete");
                        coord.x = tempCoord.x;
                        return true;
                    }
                }
                break;
            case 3:
                tempCoord.z -= 1;
                if (tempCoord.z < Mathf.Min(pt1.z, pt2.z))
                {
                    Debug.Log("case 3");
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null || map[(int)tempCoord.x, (int)tempCoord.z].tag != "Floor")
                    {
                        Debug.Log("complete");
                        coord.z = tempCoord.z;
                        return true;
                    }
                }
                break;
            case 4:
                tempCoord.x -= 1;
                if (tempCoord.x < Mathf.Min(pt1.x, pt2.x))
                {
                    Debug.Log("case 4");
                    if (map[(int)tempCoord.x, (int)tempCoord.z] == null || map[(int)tempCoord.x, (int)tempCoord.z].tag != "Floor")
                    {
                        Debug.Log("complete");
                        coord.x = tempCoord.x;
                        return true;
                    }
                }
                break;
        }
        Debug.Log("failed");
        return false;
    }*/
}
