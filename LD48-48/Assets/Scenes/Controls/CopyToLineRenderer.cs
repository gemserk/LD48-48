using BezierSolution;
using UnityEngine;

namespace Scenes.Controls
{
    [ExecuteInEditMode]
    public class CopyToLineRenderer : MonoBehaviour
    {
        public BezierSpline bezier;

        public LineRenderer lineRenderer;

        public MeshFilter meshFilter;

        public MeshCollider meshCollider;

        public int multiplyPoints = 1;

        public Vector3 offset;

        private void LateUpdate()
        {
            if (bezier == null)
                return;
            
            var count = bezier.Count;
            
            lineRenderer.enabled = true;
            lineRenderer.positionCount = count * multiplyPoints;
            // var positions = new Vector3[count];
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var pointT = (i / (float) multiplyPoints) / (float) count;
                lineRenderer.SetPosition(i, bezier.GetPoint(pointT) + offset);
                // positions[i] = bezier[i].position;
            }
            // lineRenderer.SetPositions(positions);

            if (meshFilter != null)
            {
                var mesh = new Mesh();
                lineRenderer.BakeMesh(mesh);
                meshFilter.sharedMesh = mesh;
                lineRenderer.enabled = false;

                meshCollider.sharedMesh = mesh;
            }

            lineRenderer.positionCount = 0;
        }
    }
}