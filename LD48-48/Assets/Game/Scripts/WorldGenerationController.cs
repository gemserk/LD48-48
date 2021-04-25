using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

namespace Game.Scripts
{
    public class WorldGenerationController : MonoBehaviour
    {
        // need it for next generation...
        public MineCartController mineCartController;

        public TrackRandomGenerator trackGenerator;

        public GameObject mineTrackPrefab;

        public GameObject mineTrackMeshGenerator;
        
        public bool splineAutogenerateNormals;

        private TrackMeshGenerator meshGenerator;

        private void Start()
        {
            meshGenerator = mineTrackMeshGenerator.GetComponent<TrackMeshGenerator>();
            mineCartController.DettachFromTrack();
            StartCoroutine(WorldGenerationOverTime());
        }

        private IEnumerator WorldGenerationOverTime()
        {
            var mineTrackObject = Instantiate(mineTrackPrefab);
            var mineTrack = mineTrackObject.GetComponent<MineCartTrack>();

            var points = trackGenerator.GeneratePoints(mineCartController.transform.position);
            
            CopyToSpline(mineTrack.spline, points);
            meshGenerator.GenerateMesh(mineTrack);

            yield return null;
        }

        private void CopyToSpline(BezierSpline spline, IReadOnlyList<Vector3> points)
        {
            spline.Initialize(points.Count);
            for (var index = 0; index < spline.Count; index++)
            {
                var splinePoint = spline[index];
                var position = points[index];
                splinePoint.position = position;
            }
        
            spline.AutoConstructSpline2();
            if (splineAutogenerateNormals)
                spline.AutoCalculateNormals();
        }
    }
}