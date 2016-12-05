using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public Map_Tile tilePrefab;
    public Room roomPrefab;
    public Teleporter teleporterPrefab;
    public Player playerPrefab;

    private int sizeX;
    private int sizeZ;
    private int minRoomNum = 6;
    private GameObject roomList;
    private GameObject portList;
    public Map_Tile[,] map;

    public void GenerateMap(int level)
    {
        int totalNumRooms = minRoomNum + level;

        orderObjects();
        
        sizeX = totalNumRooms * 10;
        sizeZ = totalNumRooms * 10;
        
        setMap(sizeX, sizeZ);

        createRooms(totalNumRooms);
        // Populate map
        tempPlacePlayer();
    }

    private void orderObjects()
    {
        roomList = new GameObject();

        roomList.transform.parent = transform;
        roomList.transform.localPosition = transform.localPosition;
        roomList.name = "Room List";

        portList = new GameObject();

        portList.transform.parent = transform;
        portList.transform.localPosition = transform.localPosition;
        portList.name = "Teleporters";
    }

    private void setMap(int maxX, int maxZ)
    {
        map = new Map_Tile[maxX, maxZ];

        for(int x = 0; x < maxX; x++)
        {
            for(int z = 0; z < maxZ; z++)
            {
                map[x, z] = null;
            }
        }
    }

    private void createRooms(int numRooms)
    {
        for(int i = 0; i < numRooms; i++)
        {
            Room newRoom = Instantiate(roomPrefab) as Room;
            newRoom.transform.parent = roomList.transform;
            newRoom.name = "Room " + (i + 1);
            minimizeOverlap(newRoom, numRooms);
            addTilesToRoom(newRoom);
        }

        createTeleporters(numRooms);
    }
    
    private void minimizeOverlap(Room room, int numRooms)
    {
        Room[] allRooms = roomList.GetComponentsInChildren<Room>();
        Coordinates areaPT = new Coordinates(Random.Range(1, sizeX), Random.Range(1, sizeZ));
        bool allClear = false;
        int index = 0;

        room.setCoord(areaPT, numRooms, sizeX, sizeZ);
        
        while(!allClear)
        {
            if(allRooms[index].name == room.name)
            {
                allClear = true;
            }
            if(!allClear)
            {
                float centerOffset = (numRooms - 1) * 0.5f;
                Vector2 currentRoomCenter = room.getCenter();
                Vector2 testRoomCenter = allRooms[index].getCenter();
                Vector2 distance = testRoomCenter - currentRoomCenter;

                if(Mathf.Abs(distance.magnitude) <= centerOffset)
                {
                    areaPT = new Coordinates(Random.Range(1, sizeX), Random.Range(1, sizeZ));
                    room.setCoord(areaPT, numRooms, sizeX, sizeZ);
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
        }
    }

    private void addTilesToRoom(Room room)
    {
        int totalAvailableTiles = (Mathf.Max(room.getXCoord(), room.getXBound()) - Mathf.Min(room.getXCoord(), room.getXBound())) * (Mathf.Max(room.getZCoord(), room.getZBound()) - Mathf.Min(room.getZCoord(), room.getZBound()));
        int tilesInRoom = Random.Range(Mathf.RoundToInt(totalAvailableTiles * 0.5f), Mathf.RoundToInt(totalAvailableTiles * 0.75f) + 1);

        Coordinates addCoord = new Coordinates(Random.Range(Mathf.Min(room.getXCoord(), room.getXBound()), Mathf.Max(room.getXCoord(), room.getXBound()) + 1), Random.Range(Mathf.Min(room.getZCoord(), room.getZBound()), Mathf.Max(room.getZCoord(), room.getZBound()) + 1));

        while (map[addCoord.x, addCoord.z] != null)
        {
            addCoord = null;
            addCoord = new Coordinates(Random.Range(Mathf.Min(room.getXCoord(), room.getXBound()), Mathf.Max(room.getXCoord(), room.getXBound()) + 1), Random.Range(Mathf.Min(room.getZCoord(), room.getZBound()), Mathf.Max(room.getZCoord(), room.getZBound()) + 1));
        }

        Coordinates coordCheck = new Coordinates(addCoord.x, addCoord.z);
        int index = 0;
        while(index < tilesInRoom)
        {
            createTile(addCoord, room);
            addCoord = chooseNextTile(addCoord, room);

            if (coordCheck.x == addCoord.x && coordCheck.z == addCoord.z)
            {
                Map_Tile[] tiles = room.GetComponentsInChildren<Map_Tile>();
                if(tiles.Length == 1)
                {
                    Destroy(tiles[0].gameObject);
                    index = 0;
                }
                else
                {
                    break;
                }
            }

            coordCheck.x = addCoord.x;
            coordCheck.z = addCoord.z;

            index += 1;
        }
    }

    private void createTile(Coordinates coord, Room room)
    {
        Map_Tile newTile = Instantiate(tilePrefab) as Map_Tile;
        map[coord.x, coord.z] = newTile;
        newTile.setCoordinates(coord.x, coord.z);
        newTile.name = "Tile " + coord.x + ", " + coord.z;
        newTile.transform.parent = room.transform;
        newTile.transform.localPosition = new Vector3(coord.x - sizeX * 0.5f + 0.5f, 0f, coord.z - sizeZ * 0.5f + 0.5f);
        newTile.tileType = 1;
    }

    private Coordinates chooseNextTile(Coordinates addCoord, Room room)
    {
        int rand;
        int[] choices = { 1, 2, 3, 4 };
        Coordinates newCoord = new Coordinates(addCoord.x, addCoord.z);

        for(int k = 0; k < 4; k++)
        {
            rand = Random.Range(k, choices.Length);
            
            if(validTile(choices[rand], newCoord, room))
            {
                return newCoord;
            }
            
            choices[rand] = choices[k];
        }
        
        Map_Tile[] nextTile = room.GetComponentsInChildren<Map_Tile>();
        for(int j = 0; j < room.transform.childCount; j++)
        {
            int[] choices2 = { 1, 2, 3, 4 };
            newCoord = nextTile[j].getCoordinates();
            for(int w = 0;  w < 4; w++)
            {
                if(validTile(choices2[w], newCoord, room))
                {
                    return newCoord;
                }
            }
        }
        
        return addCoord;
    }

    private bool validTile(int direction, Coordinates coord, Room room)
    {
        Coordinates tempCoord = new Coordinates(coord.x, coord.z);
        switch(direction)
        {
            case 1:
                // North: +z
                tempCoord.z += 1;
                if(tempCoord.z < Mathf.Max(room.getZCoord(), room.getZBound()))
                {
                    if (map[tempCoord.x, tempCoord.z] == null)
                    {
                        coord.z = tempCoord.z;
                        return true;
                    }
                }
                break;
            case 2:
                // East: +x
                tempCoord.x += 1;
                if (tempCoord.x < Mathf.Max(room.getXCoord(), room.getXBound()))
                {
                    if (map[tempCoord.x, tempCoord.z] == null)
                    {
                        coord.x = tempCoord.x;
                        return true;
                    }
                }
                break;
            case 3:
                // South: -z
                tempCoord.z -= 1;
                if (tempCoord.z > Mathf.Min(room.getZCoord(), room.getZBound()))
                {
                    if (map[tempCoord.x, tempCoord.z] == null)
                    {
                        coord.z = tempCoord.z;
                        return true;
                    }
                }
                break;
            case 4:
                // West: -x
                tempCoord.x -= 1;
                if (tempCoord.x > Mathf.Min(room.getXCoord(), room.getXBound()))
                {
                    if (map[tempCoord.x, tempCoord.z] == null)
                    {
                        coord.x = tempCoord.x;
                        return true;
                    }
                }
                break;
        }

        return false;
    }

    private void createTeleporters(int numRooms)
    {
        Room[] rooms = roomList.GetComponentsInChildren<Room>();

        for (int i = 0; i < numRooms; i++)
        {
            Map_Tile[] tiles = rooms[i].GetComponentsInChildren<Map_Tile>();
            
            roomPort(tiles[0]);
            roomPort(tiles[tiles.Length - 1]);
        }

        Teleporter[] ports = portList.GetComponentsInChildren<Teleporter>();

        setDestinations(ports, rooms);
    }
    
    private void roomPort(Map_Tile tile)
    {
        Renderer rend = tile.GetComponentInChildren<Renderer>();
        rend.material.color = Color.red;
        Teleporter newTeleporter = Instantiate(teleporterPrefab) as Teleporter;
        newTeleporter.transform.parent = portList.transform;
        newTeleporter.transform.position = tile.transform.position;
        newTeleporter.transform.position += new Vector3(0, 0.5f, 0);
        newTeleporter.name = tile.name + " Teleporter";
    }

    private void setDestinations(Teleporter[] ports, Room[] rooms)
    {
        int[] roomChoices = new int[rooms.Length];

        for(int j = 0; j < rooms.Length; j++)
        {
            roomChoices[j] = j + 1;
        }

        for(int i = 0; i < rooms.Length; i++)
        {
            int rand = Random.Range(1, 3);
            int portIndex = (i + 1) * 2 - rand;
            rand = Random.Range(i, roomChoices.Length);
            Map_Tile[] tiles = rooms[roomChoices[rand] - 1].GetComponentsInChildren<Map_Tile>();
            roomChoices[rand] = roomChoices[i];
            rand = Random.Range(1, tiles.Length - 1);
            ports[portIndex].setDestination(tiles[rand]);
            Renderer rend = tiles[rand].GetComponentInChildren<Renderer>();
            rend.material.color = Color.blue;
        }

        for(int w = 0; w < ports.Length; w++)
        {
            if(ports[w].getDestination() == null)
            {
                int rand = Random.Range(0, rooms.Length);
                Map_Tile[] tiles = rooms[rand].GetComponentsInChildren<Map_Tile>();
                rand = Random.Range(1, tiles.Length - 1);
                ports[w].setDestination(tiles[rand]);
                Renderer rend = tiles[rand].GetComponentInChildren<Renderer>();
                rend.material.color = Color.black;
            }
        }
    }

    private void tempPlacePlayer()
    {
        Room[] rooms = roomList.GetComponentsInChildren<Room>();
        int rand = Random.Range(0, rooms.Length);
        Map_Tile[] tiles = rooms[rand].GetComponentsInChildren<Map_Tile>();
        rand = Random.Range(1, tiles.Length - 1);
        Player pc = Instantiate(playerPrefab) as Player;
        pc.transform.parent = transform;
        pc.transform.position = tiles[rand].transform.position;
        pc.transform.position += new Vector3(0, 0.5f, 0);
        pc.name = "Player";
    }
}
