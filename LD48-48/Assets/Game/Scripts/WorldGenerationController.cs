using System;
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

        private void Start()
        {
            StartCoroutine(WorldGenerationOverTime());
        }

        private IEnumerator WorldGenerationOverTime()
        {
            var meshGenerator = mineTrackMeshGenerator.GetComponent<TrackMeshGenerator>();

            var mineTrackObject = Instantiate(mineTrackPrefab);
            var mineTrack = mineTrackObject.GetComponent<MineCartTrack>();

            var points = trackGenerator.GeneratePoints(mineCartController.transform.position);
            
            CopyToSpline(mineTrack.spline, points);
            meshGenerator.GenerateMesh(mineTrack);
            
            // for first generation only, attach the cart (or maybe the track should be generated below and
            // cart always starts falling)
            mineCartController.bezierWalker.spline = mineTrack.spline;

            yield return null;
        }

        private static void CopyToSpline(BezierSpline spline, IReadOnlyList<Vector3> points)
        {
            spline.Initialize(points.Count);
            for (var index = 0; index < spline.Count; index++)
            {
                var splinePoint = spline[index];
                var position = points[index];

                splinePoint.position = position;
            }
        
            spline.AutoConstructSpline2();
            spline.AutoCalculateNormals();
        }
    }
}