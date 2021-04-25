using System.Runtime.Remoting.Metadata.W3cXsd2001;
using BezierSolution;
using UnityEngine;

namespace Game.Scripts
{
    public class WallsSpawnerPlayerMonitor : MonoBehaviour
    {
        public MineCartController controller;
        public BezierSpline mainSpline;
        public WallsSpawner wallsSpawner;

        public float lookAhead;
        public float cleanBehind;
        
        public void Update()
        {
            float normalizedT;
            mainSpline.FindNearestPointTo(controller.transform.position, out normalizedT);
            var currentLength = mainSpline.GetLengthApproximately(0, normalizedT);
            var generateUntil = currentLength + lookAhead;
            wallsSpawner.generateUntil = generateUntil;
        }
    }
}