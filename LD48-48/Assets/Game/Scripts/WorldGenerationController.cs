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

        private BezierSpline masterSpline;

        private void Start()
        {
            var masterSplineObject = new GameObject("~MasterSpline")
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            
            masterSpline = masterSplineObject.AddComponent<BezierSpline>();
            masterSpline.loop = false;
            masterSpline.drawGizmos = false;
            
            meshGenerator = mineTrackMeshGenerator.GetComponent<TrackMeshGenerator>();
            mineCartController.DettachFromTrack();
            StartCoroutine(WorldGenerationOverTime());
        }

        private IEnumerator WorldGenerationOverTime()
        {
            var points = trackGenerator.GeneratePoints(mineCartController.transform.position);
            
            CopyToSpline(masterSpline, points);

            var mineTrackObject = Instantiate(mineTrackPrefab);
            var mineTrack = mineTrackObject.GetComponent<MineCartTrack>();

            mineTrack.trackMeshGenerator = meshGenerator;
            mineTrack.regenerateMeshOnLateUpdate = true;
            
            CopySplineSegment(masterSpline, mineTrack.spline, 0, 20, Vector3.zero);
            
            // StartCoroutine(RegenerateMeshForever(mineTrack));

            yield return null;    
            
            // while (true) 
            // {
            //     // if we have to regenerate, generate one, copy from main spline
            //     var mineTrackObject = Instantiate(mineTrackPrefab);
            //     var mineTrack = mineTrackObject.GetComponent<MineCartTrack>();
            //
            //     StartCoroutine(RegenerateMeshForever(mineTrack));
            //
            //     yield return null;    
            // }
            
        }

        // private IEnumerator RegenerateMeshForever(MineCartTrack track)
        // {
        //     while (true)
        //     {
        //         meshGenerator.GenerateMesh(track);
        //         yield return null;
        //     }
        // }
        
        private static void CopySplineSegment(BezierSpline sourceSpline, BezierSpline targetSpline, int start, int end, Vector3 offset)
        {
            var count = end - start;
            targetSpline.Initialize(count);
            for (var i = start; i < end; i++)
            {
                if (i > sourceSpline.Count)
                    break;
                
                var targetPoint = targetSpline[i - start];
                var sourcePoint = sourceSpline[i];

                targetPoint.position = sourcePoint.position + offset;
                targetPoint.normal = sourcePoint.normal;
                targetPoint.localRotation = Quaternion.identity;
                
                // apply noise + offset
            }
            
            // targetSpline.AutoConstructSpline2();
        }

        private void CopyToSpline(BezierSpline spline, IReadOnlyList<Vector3> points)
        {
            spline.Initialize(points.Count);
            for (var index = 0; index < spline.Count; index++)
            {
                var splinePoint = spline[index];
                var position = points[index];
                splinePoint.position = position;
                splinePoint.normal = Vector3.up;
            }
        
            spline.AutoConstructSpline2();
            if (splineAutogenerateNormals)
                spline.AutoCalculateNormals();
        }
    }
}