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
        public Collider triggerCollider;

        public InputActionReference tiltActionRef;
        public InputActionReference jumpActionRef;

        public float tiltAngle = 15.0f;
        public float jumpTiltXAngle = 25.0f;

        public float jumpTreshold = 0.1f;
        private bool attached => bezierWalker.spline != null;

        public Vector3 jumpForce = new Vector3(0, 100, 0);
        public ForceMode forceMode = ForceMode.Force;

        public Vector3 moveWhileJumpForce = new Vector3(0,0, 100);
        
        public float jumpDuration = 0.25f;

        public float jumpForwardSpeedFactor = 0.5f;
        public float jumpUpVelocityFactor = 1.0f;
        public float jumpTiltVelocityFactor = 5.0f;

        public float timeToActivateRigidBody = 0.25f;
        
        private float currentJumpDuration;
        private float currentTimeToActivateRigidBody;

        private bool waitingForColliderReattach;
        
        private void Start()
        {
            rigidBody.isKinematic = true;
            // triggerCollider.enabled = false;
        }

        private void AttachToTrack(GameObject track)
        {
            var temporaryCopyRenderer = track.GetComponent<CopyToLineRenderer>();
            if (temporaryCopyRenderer == null) 
                return;
            
            bezierWalker.spline = temporaryCopyRenderer.bezier;
                    
            float normalizedT;
            var bezierPosition =
                temporaryCopyRenderer.bezier.FindNearestPointTo(transform.position, out normalizedT);
        
            transform.position = bezierPosition;
            bezierWalker.NormalizedT = normalizedT;
                    
            bezierWalker.enabled = true;
            rigidBody.isKinematic = true;

            waitingForColliderReattach = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            var collider = other.collider;
            if (collider != null)
            {
                AttachToTrack(collider.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!waitingForColliderReattach)
                return;
            AttachToTrack(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            var temporaryCopyRenderer = other.gameObject.GetComponent<CopyToLineRenderer>();
            if (temporaryCopyRenderer != null && temporaryCopyRenderer.bezier != null)
            {
                waitingForColliderReattach = true;
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
                localEulerAngles.x = -jumpTiltXAngle;
            }
            
            modelTransform.localEulerAngles = localEulerAngles;
            
            currentJumpDuration -= Time.deltaTime;
            currentTimeToActivateRigidBody -= Time.deltaTime;

            if (jumpValue > jumpTreshold)
            {
                if (attached)
                {
                    bezierWalker.enabled = false;
                    bezierWalker.spline = null;
                    
                    rigidBody.isKinematic = false;
                    rigidBody.detectCollisions = false;
                    
                    // attached = false;
                    
                    // set moving velocity (in x at lease

                    if (forceMode == ForceMode.Impulse)
                    {
                        rigidBody.AddForce(jumpForce, forceMode);
                    }

                    currentJumpDuration = jumpDuration;
                    currentTimeToActivateRigidBody = timeToActivateRigidBody;

                    var forwardVelocity = transform.forward.normalized * bezierWalker.speed * jumpForwardSpeedFactor;
                    var verticalVelocity = Vector3.up * jumpUpVelocityFactor;

                    var tiltVelocity = tiltVector.x * transform.right * jumpTiltVelocityFactor;
                    
                    // initial velocity while starting jump
                    rigidBody.velocity = forwardVelocity + verticalVelocity + tiltVelocity;
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
                if (currentTimeToActivateRigidBody < 0)
                {
                    rigidBody.detectCollisions = true;
                    // triggerCollider.enabled = true;
                }

                var playerEuler = transform.localEulerAngles;
                playerEuler.x = 0;
                transform.localEulerAngles = playerEuler;
                
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