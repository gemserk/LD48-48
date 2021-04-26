using BezierSolution;
using UnityEngine;

namespace Game.Scripts
{
    public class MineCartTrack : MonoBehaviour
    {
        public MeshFilter meshFilter;
        public MeshCollider meshCollider;
        public MeshRenderer meshRenderer;
        public BezierSpline spline;
        public LineRenderer lineRenderer;

        public Color turnedOffColor;
        public Color turnedOnColor;

        public float turnedOnDuration;

        private float turnedOnCurrentTime;

        public bool regenerateMeshOnLateUpdate = true;
        
        public TrackMeshGenerator trackMeshGenerator;

        public void DisableCollisions()
        {
            meshCollider.enabled = false;
        }

        private void LateUpdate()
        {
            if (trackMeshGenerator != null && regenerateMeshOnLateUpdate)
            {
                trackMeshGenerator.GenerateMesh(this);
            }

            if (lineRenderer.positionCount < spline.Count)
            {
                trackMeshGenerator.Configure(spline, meshCollider, lineRenderer);
            }

            turnedOnCurrentTime -= Time.deltaTime;
            
            if (turnedOnCurrentTime < 0)
            {
                lineRenderer.material.SetColor("_EmissionColor", turnedOffColor);
            }
            else
            {
                lineRenderer.material.SetColor("_EmissionColor", turnedOnColor);
            }
        }

        public void SetAttachedColor(Color color)
        {
            turnedOnColor = color;
            turnedOnCurrentTime = turnedOnDuration;
        }
    }
}