using System.Collections.Generic;
using BezierSolution;
using UnityEngine;
using UnityTemplateProjects;

public class WallsSpawner : MonoBehaviour
{
    public BezierSpline spline;

    public List<GameObject> floorPrefabs;

    public float floorWidth;
    public float floorMinDistance;
    public float floorMaxDistance;
    public float floorPrefabOffsetX;
    public float floorPrefabDistanceBetween;
    [Min(0.001f)]
    public float floorDeltaMovement;

    public float floorSpawnChance;
    
    public float lightsDistanceBetween;
    public List<GameObject> lightPrefabs;
    public float lightOffsetY;
    private float lightLastSpawnDistance = 0;
    
    float floorNormalizedT = 0;
    int floorIteration = 0;
    float floorCummulativeDistance = 0;

    public float wallDistanceFromPathMin;
    public float wallDistanceFromPathMax;
    [Min(0.001f)]
    public float wallDeltaMovement;
    public float wallSpawnChance;
    public List<GameObject> wallPrefabs;
    public float wallYPos;

    float wallNormalizedT = 0;
    float wallCummulativeDistance = 0;

    public bool autoGenerate = true;
    
    public float generateUntil;

    public void Awake()
    {
        if (spline == null)
            return;
        
        if (autoGenerate)
        {
            generateUntil = spline.Length;
        }
    }

    public void Update()
    {
        if (generateUntil == 0)
            return;
        
        GenerateFloor();
        GenerateWalls();
    }

    private void GenerateFloor()
    {
        if (Mathf.Approximately(0, floorDeltaMovement))
            return;

        if (floorCummulativeDistance > generateUntil)
            return;
        
       
        int cantFloorPrefabs = Mathf.RoundToInt(floorWidth / floorPrefabDistanceBetween);
        
        List<GameObject> newFloors = new List<GameObject>(cantFloorPrefabs);
        
        while (floorNormalizedT < 1 && floorCummulativeDistance < generateUntil)
        {
            newFloors.Clear();
            Vector3 splinePos = spline.MoveAlongSpline(ref floorNormalizedT, floorDeltaMovement);

            var startX = splinePos.x - floorWidth / 2f;
            var zPos = splinePos.z;
            for (int i = 0; i < cantFloorPrefabs; i++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < floorSpawnChance)
                {
                    var xPos = startX + floorPrefabDistanceBetween * i + (floorPrefabOffsetX * floorIteration);
                    float yPos = splinePos.y + (-UnityEngine.Random.Range(floorMinDistance, floorMaxDistance));
                    var newFloor = GameObject.Instantiate(floorPrefabs.RandomItem(), new Vector3(xPos, yPos, zPos), Quaternion.identity);

                    newFloors.Add(newFloor);
                }
            }
            
            if ((floorCummulativeDistance - lightLastSpawnDistance) > lightsDistanceBetween && lightPrefabs.Count > 0 && newFloors.Count > 0)
            {
                var floor = newFloors.RandomItem();
                var floorPos = floor.transform.position;

                var lightPrefab = lightPrefabs.RandomItem();
                GameObject.Instantiate(lightPrefab, new Vector3(floorPos.x, floorPos.y + lightOffsetY, floorPos.z), Quaternion.identity);
                lightLastSpawnDistance = floorCummulativeDistance;
            }

            floorIteration = (floorIteration + 1) % 2;
            floorCummulativeDistance += floorDeltaMovement;
        }
    }
    
    private void GenerateWalls()
    {
        if (Mathf.Approximately(0, wallDeltaMovement))
            return;
        
        if (wallCummulativeDistance > generateUntil)
            return;

        
        var wallMultipliers = new int[] {-1, 1};
        while (wallNormalizedT < 1 && wallCummulativeDistance < generateUntil)
        {
            Vector3 splinePos = spline.MoveAlongSpline(ref wallNormalizedT, wallDeltaMovement);
            
            foreach (var wallMultiplier in wallMultipliers)
            {
                if (UnityEngine.Random.Range(0f, 1f) < wallSpawnChance)
                {
                    var distanceFromPath = UnityEngine.Random.Range(wallDistanceFromPathMin, wallDistanceFromPathMax);
                    var xPos = splinePos.x + distanceFromPath * wallMultiplier;
                    var zPos = splinePos.z;
                    var yPos = splinePos.y + wallYPos;
                    GameObject.Instantiate(wallPrefabs.RandomItem(), new Vector3(xPos, yPos, zPos), Quaternion.identity);
                }
            }

            wallCummulativeDistance = +wallDeltaMovement;
        }
    }
}
