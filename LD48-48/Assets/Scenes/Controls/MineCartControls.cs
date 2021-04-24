using BezierSolution;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes.Controls
{
    public class MineCartControls : MonoBehaviour
    {
        public BezierWalkerWithSpeed bezierWalker;

        public Transform modelTransform;
        public Rigidbody rigidBody;

        public PlayerInput playerInput;

        public InputActionReference tiltActionRef;
        public InputActionReference jumpActionRef;

        public float tiltAngle = 15.0f;

        private void Start()
        {
            rigidBody.isKinematic = true;
        }

        private void Update()
        {
            var tiltAction = tiltActionRef.action;
            var tiltVector = tiltAction.ReadValue<Vector2>();

            var localEulerAngles = modelTransform.localEulerAngles;
            localEulerAngles.z = -tiltVector.x * tiltAngle;
            modelTransform.localEulerAngles = localEulerAngles;
            
            // Debug.Log(tiltVector);
            // tiltLeft.
        }
    }
}