using BezierSolution;
using UnityEngine;

namespace Game.Scripts
{
	public class TrackMeshGenerator : MonoBehaviour
    {
        // Used to generate the mesh
        
        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private int multiplyPoints = 10;

        [SerializeField]
        private Vector3 offset = new Vector3(0, -0.5f, 0);

        private void Awake()
        {
            lineRenderer.enabled = false;
        }

        public static Mesh GenerateMeshStatic(BezierSpline spline, MeshFilter meshFilter, MeshCollider meshCollider)
        {
            var meshGenerator = FindObjectOfType<TrackMeshGenerator>();
            return meshGenerator.GenerateMesh(spline, meshFilter, meshCollider);
        }

        public Mesh GenerateMesh(BezierSpline spline, MeshFilter meshFilter, MeshCollider meshCollider)
        {
            var count = spline.Count;
            
            lineRenderer.enabled = true;
            lineRenderer.positionCount = count * multiplyPoints;
            
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var pointT = (i / (float) multiplyPoints) / (float) count;
                lineRenderer.SetPosition(i, spline.GetPoint(pointT) + offset);
            }
            
            var mesh = new Mesh();
            lineRenderer.BakeMesh(mesh);
            meshFilter.sharedMesh = mesh;
            lineRenderer.enabled = false;
            meshCollider.sharedMesh = mesh;

            lineRenderer.positionCount = 0;
            lineRenderer.enabled = false;

            return mesh;
        }
    }
}