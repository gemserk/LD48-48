using System;
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
        private bool attached => bezierWalker.spline != null;

        public Vector3 jumpForce = new Vector3(0, 100, 0);
        public ForceMode forceMode = ForceMode.Force;

        public Vector3 moveWhileJumpForce = new Vector3(0,0, 100);
        
        public float jumpDuration = 0.25f;

        public float jumpForwardSpeedFactor = 0.5f;
        public float jumpUpVelocityFactor = 1.0f;

        // public float timeToActivateRigidBody = 0.25f;
        
        private float currentJumpDuration;
        
        private void Start()
        {
            rigidBody.isKinematic = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            var collider = other.collider;
            if (collider != null)
            {
                var temporaryCopyRenderer = collider.gameObject.GetComponentInChildren<CopyToLineRenderer>();
                if (temporaryCopyRenderer != null)
                {
                    bezierWalker.spline = temporaryCopyRenderer.bezier;
                    
                    float normalizedT;
                    var bezierPosition =
                        temporaryCopyRenderer.bezier.FindNearestPointTo(transform.position, out normalizedT);

                    transform.position = bezierPosition;
                    bezierWalker.NormalizedT = normalizedT;
                    
                    bezierWalker.enabled = true;
                    rigidBody.isKinematic = true;
                }
            }
        }

        private void FixedUpdate()
        {
            var tiltVector = tiltActionRef.action.ReadValue<Vector2>();
            var jumpValue = jumpActionRef.action.ReadValue<float>();

            // tilt logic while attached to track
            var localEulerAngles = modelTransform.localEulerAngles;
            localEulerAngles.z = -tiltVector.x * tiltAngle;

            if (!attached)
            {
                localEulerAngles.x = -tiltAngle;
            }
            
            modelTransform.localEulerAngles = localEulerAngles;
            
            currentJumpDuration -= Time.deltaTime;

            if (jumpValue > jumpTreshold)
            {
                if (attached)
                {
                    bezierWalker.enabled = false;
                    bezierWalker.spline = null;
                    
                    rigidBody.isKinematic = false;
                    // attached = false;
                    
                    // set moving velocity (in x at lease

                    if (forceMode == ForceMode.Impulse)
                    {
                        rigidBody.AddForce(jumpForce, forceMode);
                    }

                    currentJumpDuration = jumpDuration;

                    var forwardVelocity = transform.forward.normalized * bezierWalker.speed * jumpForwardSpeedFactor;
                    var verticalVelocity = Vector3.up * jumpUpVelocityFactor;
                    
                    // initial velocity while starting jump
                    rigidBody.velocity = forwardVelocity + verticalVelocity;
                }
                
                if (currentJumpDuration > 0)
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
                // localEulerAngles = modelTransform.localEulerAngles;
                // localEulerAngles.x = -tiltAngle;
                // modelTransform.localEulerAngles = localEulerAngles;
                rigidBody.AddForce(tiltVector.x * moveWhileJumpForce * Time.deltaTime, ForceMode.Force);
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