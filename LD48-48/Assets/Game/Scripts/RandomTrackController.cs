using UnityEngine;

public class RandomTrackController : MonoBehaviour
{
    public TrackRandomGenerator trackRandomGenerator;
    public WallsSpawner wallsSpawner;

    public GameObject trackPrefab;

    private void Awake()
    {
        var spline = wallsSpawner.spline;
        var points = trackRandomGenerator.GeneratePoints(spline.transform.position);
        spline.Initialize(points.Count);
        for (var index = 0; index < spline.Count; index++)
        {
            var splinePoint = spline[index];
            var position = points[index];

            splinePoint.position = position;
        }

        spline.loop = false;
        spline.AutoConstructSpline2();
        // spline.AutoCalculateNormals();
        
       // wallsSpawner.Generate();
    }
}