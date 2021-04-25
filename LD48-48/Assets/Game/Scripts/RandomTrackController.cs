
using System;
using BezierSolution;
using UnityEngine;

public class RandomTrackController : MonoBehaviour
{
    public TrackRandomGenerator trackRandomGenerator;
    public WallsSpawner wallsSpawner;

    private void Awake()
    {
        BezierSpline spline = wallsSpawner.spline;
        var points = trackRandomGenerator.GeneratePoints(spline.transform.position);
        spline.Initialize(points.Count);
        for (var index = 0; index < spline.Count; index++)
        {
            var splinePoint = spline[index];
            var position = points[index];

            splinePoint.position = position;
        }
        
        spline.AutoConstructSpline2();
        
        wallsSpawner.Generate();
    }
}