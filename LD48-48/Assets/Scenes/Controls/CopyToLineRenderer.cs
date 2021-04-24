using BezierSolution;
using UnityEngine;

namespace Scenes.Controls
{
    [ExecuteInEditMode]
    public class CopyToLineRenderer : MonoBehaviour
    {
        public BezierSpline bezier;

        public LineRenderer lineRenderer;

        public int multiplyPoints = 1;

        public Vector3 offset;

        // Update is called once per frame
        // [ContextMenu("Regenerate")]
        private void LateUpdate()
        {
            var count = bezier.Count;
            lineRenderer.positionCount = count * multiplyPoints;
            // var positions = new Vector3[count];
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var pointT = (i / (float) multiplyPoints) / (float) count;
                lineRenderer.SetPosition(i, bezier.GetPoint(pointT) + offset);
                // positions[i] = bezier[i].position;
            }
            // lineRenderer.SetPositions(positions);
        }
    }
}