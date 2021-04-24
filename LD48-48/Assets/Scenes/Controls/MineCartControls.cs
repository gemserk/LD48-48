using BezierSolution;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes.Controls
{
    public class MineCartControls : MonoBehaviour
    {
        public BezierWalkerWithSpeed bezierWalker;

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
            // var tiltAction = playerInput.actions[tiltActionName];
            var tiltAction = tiltActionRef.action;
            // playerInput.actions[tiltActionRef.action]
            var tiltVector = tiltAction.ReadValue<Vector2>();

            var localEulerAngles = transform.localEulerAngles;
            localEulerAngles.z = tiltVector.x * tiltAngle;
            transform.localEulerAngles = localEulerAngles;
            
            // Debug.Log(tiltVector);
            // tiltLeft.
        }
    }
}