using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class MineCartController : MonoBehaviour
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
        
        public ParticleAttachPoint attachToTrackParticle;

        private MineCartTrack currentMineCartTrack;
        
        private void Start()
        {
            // rigidBody.isKinematic = true;
            
            if (controlsAsset.overrideRigidBodyMass > 0)
                rigidBody.mass = controlsAsset.overrideRigidBodyMass;

            if (bezierWalker == null)
            {
                bezierWalker = GetComponent<BezierWalkerWithSpeedFixedUpdate>();
            }
            
            // Start detached from tracks by default and expect to auto attach
            // DettachFromTrack();
            
            bezierWalker.onPathCompleted.AddListener(OnPathCompleted);
        }

        private void OnPathCompleted()
        {
            if (currentMineCartTrack != null)
            {
                currentMineCartTrack.DisableCollisions();
                DettachFromTrack();
            }
        }

        public void DettachFromTrack()
        {
            bezierWalker.enabled = false;
            bezierWalker.spline = null;
                    
            rigidBody.isKinematic = false;
            rigidBody.detectCollisions = false;

            currentTimeToActivateRigidBody = 0;

            currentMineCartTrack = null;
        }

        private void AttachToTrack(GameObject trackObject)
        {
            var mineCartTrack = trackObject.GetComponentInParent<MineCartTrack>();
            if (mineCartTrack == null) 
                return;
            
            bezierWalker.spline = mineCartTrack.spline;
                    
            float normalizedT;
            
            var bezierPosition =
                mineCartTrack.spline.FindNearestPointTo(transform.position, out normalizedT);
        
            transform.position = bezierPosition;
            bezierWalker.NormalizedT = normalizedT;
                    
            bezierWalker.enabled = true;
            rigidBody.isKinematic = true;

            waitingForColliderReattach = false;

            if (attachToTrackParticle != null)
            {
                attachToTrackParticle.Spawn();
            }

            currentMineCartTrack = mineCartTrack;
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
                    DettachFromTrack();
                    
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

            if (attached)
            {
                var forwardAngle = transform.localEulerAngles.x;
                if (forwardAngle > 180)
                {
                    forwardAngle -= 360;
                }
                var t = (forwardAngle + 90.0f) / 180.0f;
                
                var speedUpFactor = (tiltVector.y + 1.0f) * 0.5f;
                var speedMultiplier = Mathf.Lerp(controlsAsset.tiltMinTravelSpeedMultiplier, 
                    controlsAsset.tiltMaxTravelSpeedMultiplier, speedUpFactor);
                
                bezierWalker.speed = Mathf.Lerp(controlsAsset.slopeMinTravelSpeed, controlsAsset.slopeMaxTravelSpeed, t) 
                                     * speedMultiplier;

                // Debug.Log($"{t}, {bezierWalker.speed}, {forwardAngle}, {speedMultiplier}");
                
                localEulerAngles = modelTransform.localEulerAngles;
                localEulerAngles.x = tiltVector.y * controlsAsset.forwardTiltAngle;
                modelTransform.localEulerAngles = localEulerAngles;

            } else 
            {
                if (currentTimeToActivateRigidBody < 0)
                {
                    rigidBody.detectCollisions = true;
                    // triggerCollider.enabled = true;
                }

                // var playerEuler = transform.localEulerAngles;
                // playerEuler.x = 0;
                // transform.localEulerAngles = playerEuler;
                
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

        private void OnDrawGizmosSelected()
        {
            if (bezierWalker != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * bezierWalker.speed);
            }
        }
    }
}