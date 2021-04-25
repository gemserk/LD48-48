
using System.Collections.Generic;
using UnityEngine;

public class TrackRandomGenerator : MonoBehaviour
{
    public int steps;
    public float stepDistanceMin;
    public float stepDistanceMax;
    public float descentPerStep;
    public Vector2 maxRandomOffset;
    public Vector2 minRandomOffset;

    public List<Vector3> GeneratePoints(Vector3 from)
    {
        var points = new List<Vector3>();
        points.Add(from);

        for (var i = 0; i < steps; i++)
        {
            var lastPoint = points[i];
            var newPoint = lastPoint + new Vector3(0, descentPerStep, UnityEngine.Random.Range(stepDistanceMin, stepDistanceMax));

            var offset = new Vector3(UnityEngine.Random.Range(minRandomOffset.x, maxRandomOffset.x), UnityEngine.Random.Range(minRandomOffset.y, maxRandomOffset.y),0);

            var offsetScale = new Vector3(UnityEngine.Random.Range(0, 1f) < 0.5 ? -1 : 1, UnityEngine.Random.Range(0, 1f) < 0.5 ? -1 : 1, 1);

            offset.x *= offsetScale.x;
            offset.y *= offsetScale.y;
            
            newPoint = newPoint + offset;
            points.Add(newPoint);
        }
        
        return points;
    } 
}