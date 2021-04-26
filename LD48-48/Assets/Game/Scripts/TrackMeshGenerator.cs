using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

namespace Game.Scripts
{
	public class TrackMeshGenerator : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private int multiplyPoints = 10;

        [SerializeField]
        private Vector3 offset = new Vector3(0, -0.5f, 0);
        
        public static Mesh GenerateMeshStatic(BezierSpline spline, MeshFilter meshFilter, MeshCollider meshCollider)
        {
            var meshGenerator = FindObjectOfType<TrackMeshGenerator>();
            return meshGenerator.GenerateMesh(spline, meshFilter, meshCollider, new List<Vector3>());
        }

        public Mesh GenerateMesh(MineCartTrack track)
        {
            return GenerateMesh(track.spline, track.meshFilter, track.meshCollider, new List<Vector3>());
        }

        public Mesh GenerateMesh(BezierSpline spline, MeshFilter meshFilter, MeshCollider meshCollider, List<Vector3> pointCache)
        {
            var count = spline.Count;
            
            lineRenderer.positionCount = count * multiplyPoints;
            
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var pointT = (i / (float) multiplyPoints) / (float) count;
                lineRenderer.SetPosition(i, spline.GetPoint(pointT) + offset);
            }

            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.MarkDynamic();
            }
            
            lineRenderer.BakeMesh(mesh);
            meshFilter.sharedMesh = mesh;
            if (meshCollider.sharedMesh == null)
            {
                meshCollider.sharedMesh = mesh;
            }

            lineRenderer.positionCount = 0;

            return mesh;
        }
        
        public void Configure(BezierSpline spline, MeshCollider meshCollider, LineRenderer trackLineRenderer)
        {
            var count = spline.Count;
            
            trackLineRenderer.positionCount = count * multiplyPoints;
            
            for (var i = 0; i < trackLineRenderer.positionCount; i++)
            {
                var pointT = (i / (float) multiplyPoints) / (float) count;
                trackLineRenderer.SetPosition(i, spline.GetPoint(pointT) + offset);
            }

            Mesh mesh = new Mesh();
            mesh.MarkDynamic();
            
            
            trackLineRenderer.BakeMesh(mesh);
            
            meshCollider.sharedMesh = mesh;
        }
    }
}