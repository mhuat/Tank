using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Globalization;
using Random = UnityEngine.Random;

//using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public NavMeshSurface surface;
    public int r, c; //rows, columns
    [SerializeField]
    private int seed;
    private float w =100, h=100; //Width, Height
    [Header("List of Tile Types")]
    public List<GameObject> tiles = new List<GameObject>();
    //public GameObject tile;
    public bool generateMd;

    private void Awake()
    {
        string day = Convert.ToString(DateTime.Today.Day);
        string month = Convert.ToString(DateTime.Today.Month);
        string year = Convert.ToString(DateTime.Today.Year);
        seed = Convert.ToInt32(Convert.ToString(month + day + year));
        if (generateMd)
        {
            Debug.Log(seed);
        }
    }

    private void Start(){
        if (generateMd) GenerateMapOfTheDay(); else GenerateMap();    
    }
    
    void GenerateMap()
    {
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                float xPos = w * j;
                float zPos = h * i;
                Vector3 newPosition = new Vector3(xPos, 0.0f, zPos);
                Instantiate(tiles[Random.Range(0, tiles.Count)], newPosition, Quaternion.identity);
            }
        }
        surface.BuildNavMesh();
    }

    void GenerateMapOfTheDay()
    {
        string day = Convert.ToString(DateTime.Today.Day);
        string month = Convert.ToString(DateTime.Today.Month);
        string year = Convert.ToString(DateTime.Today.Year);
        int monthlyInt = DateTime.Today.Month;
        seed = Convert.ToInt32(Convert.ToString(month + day + year)); // Designers will not touch my seed.
        r = Convert.ToInt32(day);
        c = Convert.ToInt32(month) + monthlyInt;
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                float xPos = w * j;
                float zPos = h * i;
                Vector3 newPosition = new Vector3(xPos, 0.0f, zPos);
                Instantiate(tiles[0], newPosition, Quaternion.identity);
            }
        }
        surface.BuildNavMesh();
    }


    /*
    int x;
    int y;
    public void Math(int x, int y)
    {
        int z = x + y;
        return;
    }*/
}