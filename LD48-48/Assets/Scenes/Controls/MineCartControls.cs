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

        public InputActionReference tiltActionRef;
        public InputActionReference jumpActionRef;

        public float tiltAngle = 15.0f;

        public float jumpTreshold = 0.1f;
        private bool attached = true;

        public Vector3 jumpForce = new Vector3(0, 100, 0);
        public ForceMode forceMode = ForceMode.Force;

        public float jumpDuration = 0.25f;

        private float currentJumpDuration;

        private void Start()
        {
            rigidBody.isKinematic = true;
        }

        private void Update()
        {
            var tiltVector = tiltActionRef.action.ReadValue<Vector2>();
            var jumpValue = jumpActionRef.action.ReadValue<float>();

            // tilt logic while attached to track
            var localEulerAngles = modelTransform.localEulerAngles;
            localEulerAngles.z = -tiltVector.x * tiltAngle;
            modelTransform.localEulerAngles = localEulerAngles;
            
            jumpDuration -= Time.deltaTime;

            // if (attached)
            // {
            //
            // }
            // else
            // {
            //     // tilt in air is move right/left or something alos
            // }

            if (jumpValue > jumpTreshold)
            {
                if (attached)
                {
                    bezierWalker.enabled = false;
                    rigidBody.isKinematic = false;
                    attached = false;
                    
                    // set moving velocity (in x at lease)
                    
                    if (forceMode == ForceMode.Impulse)
                    {
                        rigidBody.AddForce(jumpForce, forceMode);
                    }

                    currentJumpDuration = jumpDuration;
                }
                
                if (jumpDuration > 0)
                {
                    // TODO: apply force given normal of the bezier, not just vector up

                    if (forceMode != ForceMode.Impulse)
                    {
                        rigidBody.AddForce(jumpForce * Time.deltaTime, forceMode);
                    }
                }
            }

            if (!attached)
            {
                
            }

            // if (jumpActionRef.action.triggered)
            // {
            //     
            // }
            

            // Debug.Log(tiltVector);
            // tiltLeft.
        }
    }
}