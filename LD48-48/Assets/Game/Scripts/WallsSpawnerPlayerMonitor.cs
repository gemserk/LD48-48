using BezierSolution;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class WallsSpawnerPlayerMonitor : MonoBehaviour
    {
        public MineCartController controller;
        public BezierSpline mainSpline;
        public WallsSpawner wallsSpawner;
        public Camera camera;

        public float lookAhead;
        public float cleanBehind;

        public UnityEvent onDeathEvent;
        public float yOffsetDeath;
        
        public void Update()
        {
            wallsSpawner.spline = mainSpline;
            
            float normalizedT;
            mainSpline.FindNearestPointTo(controller.transform.position, out normalizedT);
            var currentLength = mainSpline.GetLengthApproximately(0, normalizedT);
            var generateUntil = currentLength + lookAhead;
            wallsSpawner.generateUntil = generateUntil;
            wallsSpawner.cleanBehind = camera.transform.position.z - cleanBehind;

            CheckDeath(mainSpline.GetPoint(normalizedT));
        }

        private void CheckDeath(Vector3 pos)
        {
            if (controller.transform.position.y  < pos.y + yOffsetDeath)
            {
                onDeathEvent.Invoke();
            }
        }

        public void Restart()
        {
            wallsSpawner.generateUntil = 0;
            wallsSpawner.Restart();
        }
    }
}