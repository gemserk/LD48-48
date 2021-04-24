using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

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

    public float wallDistanceFromPathMin;
    public float wallDistanceFromPathMax;
    [Min(0.001f)]
    public float wallDeltaMovement;
    public float wallSpawnChance;
    public List<GameObject> wallPrefabs;
    public float wallYPos;
    

    public void Awake()
    {
        GenerateFloor();
        GenerateWalls();
    }

    private void GenerateFloor()
    {
        if (Mathf.Approximately(0, floorDeltaMovement))
            return;
        
        float normalizedT = 0;
        int iteration = 0;
        while (normalizedT < 1)
        {
            Vector3 splinePos = spline.MoveAlongSpline(ref normalizedT, floorDeltaMovement);

            int cantFloorPrefabs = Mathf.RoundToInt(floorWidth / floorPrefabDistanceBetween);

            var startX = splinePos.x - floorWidth / 2f;
            var zPos = splinePos.z;
            for (int i = 0; i < cantFloorPrefabs; i++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < floorSpawnChance)
                {
                    var xPos = startX + floorPrefabDistanceBetween * i + (floorPrefabOffsetX * iteration);
                    float yPos = splinePos.y + (-UnityEngine.Random.Range(floorMinDistance, floorMaxDistance));
                    GameObject.Instantiate(floorPrefabs[0], new Vector3(xPos, yPos, zPos), Quaternion.identity);
                }
            }

            iteration = (iteration + 1) % 2;
        }
    }
    
    private void GenerateWalls()
    {
        if (Mathf.Approximately(0, wallDeltaMovement))
            return;
        
        float normalizedT = 0;
        var wallMultipliers = new int[] {-1, 1};
        while (normalizedT < 1)
        {
            Vector3 splinePos = spline.MoveAlongSpline(ref normalizedT, wallDeltaMovement);
            
            foreach (var wallMultiplier in wallMultipliers)
            {
                if (UnityEngine.Random.Range(0f, 1f) < wallSpawnChance)
                {
                    var distanceFromPath = UnityEngine.Random.Range(wallDistanceFromPathMin, wallDistanceFromPathMax);
                    var xPos = splinePos.x + distanceFromPath * wallMultiplier;
                    var zPos = splinePos.z;
                    var yPos = splinePos.y + wallYPos;
                    GameObject.Instantiate(wallPrefabs[0], new Vector3(xPos, yPos, zPos), Quaternion.identity);
                }
            }
        }
    }
}
