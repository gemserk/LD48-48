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
            return meshGenerator.GenerateMesh(spline, meshFilter, meshCollider);
        }

        public Mesh GenerateMesh(MineCartTrack track)
        {
            return GenerateMesh(track.spline, track.meshFilter, track.meshCollider);
        }

        public Mesh GenerateMesh(BezierSpline spline, MeshFilter meshFilter, MeshCollider meshCollider)
        {
            var count = spline.Count;
            
            lineRenderer.positionCount = count * multiplyPoints;
            
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var pointT = (i / (float) multiplyPoints) / (float) count;
                lineRenderer.SetPosition(i, spline.GetPoint(pointT) + offset);
            }
            
            var mesh = new Mesh();
            lineRenderer.BakeMesh(mesh);
            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh = mesh;

            lineRenderer.positionCount = 0;

            return mesh;
        }
    }
}