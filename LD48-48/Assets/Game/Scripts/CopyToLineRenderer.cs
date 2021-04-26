using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

namespace Game.Scripts
{
    [ExecuteInEditMode]
    public class CopyToLineRenderer : MonoBehaviour
    {
        public BezierSpline bezier;

        public MeshFilter meshFilter;

        public MeshCollider meshCollider;

        public GameObject trackMeshGenerator;

        private void LateUpdate()
        {
            if (bezier == null || meshFilter == null || meshCollider == null || trackMeshGenerator == null)
                return;

            var generator = trackMeshGenerator.GetComponentInChildren<TrackMeshGenerator>();
            generator.GenerateMesh(bezier, meshFilter, meshCollider, new List<Vector3>());
        }
    }
}