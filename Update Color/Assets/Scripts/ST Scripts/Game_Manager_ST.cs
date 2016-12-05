using UnityEngine;
using System.Collections;

public class Game_Manager_ST : MonoBehaviour
{
    public Map_ST mapPrefab;

    private Map_ST mapInstance;

    // Use this for initialization
    void Start()
    {
        mapInstance = Instantiate(mapPrefab) as Map_ST;
        mapInstance.transform.localPosition = transform.localPosition;
        mapInstance.name = "Map";
        mapInstance.createMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
