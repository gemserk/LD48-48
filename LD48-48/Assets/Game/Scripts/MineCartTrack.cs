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
        }
    }
}