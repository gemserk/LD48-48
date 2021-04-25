using Game.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes.Controls
{
    public class MineCartControls : MonoBehaviour
    {
        public MineCartControlsAsset controlsAsset;
        
        public BezierWalkerWithSpeedFixedUpdate bezierWalker;

        public Transform modelTransform;
        public Rigidbody rigidBody;

        public InputActionReference tiltActionRef;
        public InputActionReference jumpActionRef;
        
        private bool attached => bezierWalker.spline != null;

        private float currentJumpDuration;
        private float currentTimeToActivateRigidBody;

        private bool waitingForColliderReattach;
        
        private void Start()
        {
            rigidBody.isKinematic = true;
            if (controlsAsset.overrideRigidBodyMass > 0)
                rigidBody.mass = controlsAsset.overrideRigidBodyMass;

            if (bezierWalker == null)
            {
                bezierWalker = GetComponent<BezierWalkerWithSpeedFixedUpdate>();
            }
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
            localEulerAngles.z = -tiltVector.x * controlsAsset.tiltAngle;

            if (!attached)
            {
                localEulerAngles.x = -controlsAsset.jumpTiltXAngle;
            }
            
            modelTransform.localEulerAngles = localEulerAngles;
            
            currentJumpDuration -= Time.deltaTime;
            currentTimeToActivateRigidBody -= Time.deltaTime;

            if (jumpValue > controlsAsset.jumpStartTreshold)
            {
                if (attached)
                {
                    bezierWalker.enabled = false;
                    bezierWalker.spline = null;
                    
                    rigidBody.isKinematic = false;
                    rigidBody.detectCollisions = false;
                    
                    // attached = false;
                    
                    // set moving velocity (in x at lease

                    if (controlsAsset.forceMode == ForceMode.Impulse)
                    {
                        rigidBody.AddForce(controlsAsset.jumpForce, controlsAsset.forceMode);
                    }

                    currentJumpDuration = controlsAsset.jumpDuration;
                    currentTimeToActivateRigidBody = controlsAsset.timeToActivateRigidBody;

                    var forwardVelocity = transform.forward.normalized * bezierWalker.speed * controlsAsset.jumpForwardSpeedFactor;
                    var verticalVelocity = Vector3.up * controlsAsset.jumpUpVelocityFactor;

                    var tiltVelocity = tiltVector.x * transform.right * controlsAsset.jumpTiltVelocityFactor;
                    
                    // initial velocity while starting jump
                    rigidBody.velocity = forwardVelocity + verticalVelocity + tiltVelocity;
                }
                
                if (currentJumpDuration > 0)
                {
                    // TODO: apply force given normal of the bezier, not just vector up

                    if (controlsAsset.forceMode != ForceMode.Impulse)
                    {
                        rigidBody.AddForce(controlsAsset.jumpForce * Time.deltaTime, controlsAsset.forceMode);
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
                rigidBody.AddForce(-tiltVector.x * controlsAsset.moveWhileJumpForce * Time.deltaTime, ForceMode.Force);

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