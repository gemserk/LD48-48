using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using Cinemachine;
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

        public WallsSpawnerPlayerMonitor wallsSpawner;
        
        public bool splineAutogenerateNormals;

        private TrackMeshGenerator meshGenerator;

        private BezierSpline masterSpline;

        public int minSegmentLength = 100;
        public int maxSegmentLength = 100;

        public Vector3 minOffsetTrack;
        public Vector3 maxOffsetTrack;

        private List<MineCartTrack> tracks = new List<MineCartTrack>();

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

            if (wallsSpawner != null)
            {
                wallsSpawner.mainSpline = masterSpline;
                wallsSpawner.controller = mineCartController;
            }
        }

        private IEnumerator WorldGenerationOverTime()
        {
           RegenerateTracks();

            // CopySplineSegment(masterSpline, mineTrack.spline, 0, 20, Vector3.zero);

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

        private void CreateMineTrack(Vector3 offset)
        {
            var mineTrackObject = Instantiate(mineTrackPrefab);
            var mineTrack = mineTrackObject.GetComponent<MineCartTrack>();

            mineTrack.trackMeshGenerator = meshGenerator;
            mineTrack.regenerateMeshOnLateUpdate = true;

            var segmentLength = UnityEngine.Random.Range(minSegmentLength, maxSegmentLength);
            
            CopySplineSegment(masterSpline, mineTrack.spline, 0, segmentLength, offset);

            tracks.Add(mineTrack);
        }

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
            
            targetSpline.AutoConstructSpline2();
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
                splinePoint.localRotation = Quaternion.identity;
            }
        
            spline.AutoConstructSpline2();
            if (splineAutogenerateNormals)
                spline.AutoCalculateNormals();
        }

        public void ReattachPlayerToNearest()
        {
            var middleTrack = tracks[0];
            //var point = middleTrack.spline.FindNearestPointTo(mineCartController.transform.position);
            var point = middleTrack.spline.GetPoint(0);
            mineCartController.transform.position = point;

            RegenerateTracks();
            middleTrack = tracks[0];
            mineCartController.bezierWalker.spline = middleTrack.spline;
            

            mineCartController.bezierWalker.NormalizedT = 0;
            mineCartController.ReactivateControls();

            var wallsSpawnerMonitor = this.GetComponentInChildren<WallsSpawnerPlayerMonitor>();
            wallsSpawnerMonitor.Restart();
        }

        public void RegenerateTracks()
        {
            tracks.ForEach(t => GameObject.Destroy(t.gameObject));
            tracks.Clear();
            
            var points = trackGenerator.GeneratePoints(mineCartController.transform.position);
            
            CopyToSpline(masterSpline, points);

            CreateMineTrack(new Vector3(0, 0, 0));
            CreateMineTrack(new Vector3(-UnityEngine.Random.Range(minOffsetTrack.x, maxOffsetTrack.x), 
                UnityEngine.Random.Range(minOffsetTrack.y, maxOffsetTrack.y), 
                UnityEngine.Random.Range(minOffsetTrack.z, maxOffsetTrack.z)));
            CreateMineTrack(new Vector3(UnityEngine.Random.Range(minOffsetTrack.x, maxOffsetTrack.x), 
                UnityEngine.Random.Range(minOffsetTrack.y, maxOffsetTrack.y), 
                UnityEngine.Random.Range(minOffsetTrack.z, maxOffsetTrack.z)));
        }
    }
}