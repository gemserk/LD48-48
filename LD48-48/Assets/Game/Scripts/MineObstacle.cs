using System;
using UnityEngine;

namespace Game.Scripts
{
    public class MineObstacle : MonoBehaviour
    {
        public Collider collider;

        public bool animateModel;

        public AnimationCurve randomSpeed;
        
        public Vector3 animationDirection;

        public AnimationCurve curve;
        public Transform modelTransform;

        private float animationSpeed;
        private float time;
        private float timeDirection = 1;

        private void Awake()
        {
            if (animateModel)
            {
                time = UnityEngine.Random.Range(0.0f, 1.0f);
                animationSpeed = randomSpeed.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f));
            }
        }

        public void DisableCollisions()
        {
            collider.enabled = false;
        }

        private void LateUpdate()
        {
            if (!animateModel)
            {
                return;
            }
            
            time += Time.deltaTime * timeDirection * animationSpeed;
            var value = curve.Evaluate(time);
            modelTransform.localPosition = animationDirection * value;

            if (time > 1.0f)
            {
                timeDirection = -1f;
            }

            if (time < 0)
            {
                timeDirection = 1.0f;
            }
        }
    }
}