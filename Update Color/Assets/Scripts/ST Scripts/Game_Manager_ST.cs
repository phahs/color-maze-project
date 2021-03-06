﻿using UnityEngine;
using System.Collections;

public class Game_Manager_ST : MonoBehaviour
{
    public Map_ST mapPrefab;

    private Map_ST mapInstance;
    private int level;

    // Use this for initialization
    void Start()
    {
        level = 1;
        makeMap();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(mapInstance.gameObject);
            makeMap();
        }
    }

    private void makeMap()
    {
        mapInstance = Instantiate(mapPrefab) as Map_ST;
        mapInstance.transform.localPosition = transform.localPosition;
        mapInstance.name = "Map";
        mapInstance.createMap(level);
    }

}
