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

        private PingPongAnimation pingPongAnimation = new PingPongAnimation();

        private void Awake()
        {
            if (animateModel)
            {
                pingPongAnimation.time = UnityEngine.Random.Range(0.0f, 1.0f);
                pingPongAnimation.animationSpeed = randomSpeed.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f));
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
            
            pingPongAnimation.Update(Time.deltaTime);
            
            var value = curve.Evaluate(pingPongAnimation.time);
            modelTransform.localPosition = animationDirection * value;
        }
    }
}